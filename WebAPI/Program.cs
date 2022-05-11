using Azure.Identity;
using Azure.Storage.Blobs;

using BlazorApp1.Application;
using BlazorApp1.Application.Services;
using BlazorApp1.Infrastructure;
using BlazorApp1.WebAPI;
using BlazorApp1.WebAPI.Hubs;
using BlazorApp1.WebAPI.Services;

using Duende.IdentityServer.Services;

using MassTransit;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Azure;

using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();
// .AddGoogle(googleOptions =>
// {
//     googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//     googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
// });
// .AddOpenIdConnect("aad", "Azure AD", options =>
// {
//     options.SignInScheme = IdentityConstants.ExternalScheme;
//     options.SignOutScheme = IdentityServerConstants.SignoutScheme;
// 
//     var tenantId = builder.Configuration["Authentication:AzureAd:TenantId"];
// 
//     options.Authority = $"https://login.windows.net/{tenantId}";
//     options.ClientId = builder.Configuration["Authentication:AzureAd:ApplicationId"];
//     options.ResponseType = OpenIdConnectResponseType.IdToken;
//     options.CallbackPath = "/signin-aad";
//     options.SignedOutCallbackPath = "/signout-callback-aad";
//     options.RemoteSignOutPath = "/signout-aad";
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         NameClaimType = "name",
//         RoleClaimType = "role"
//     };
// });

builder.Services.AddTransient<IProfileService, MyProfileService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSignalR();

//builder.Services.AddScoped<IUserIdProvider, EmailBasedUserIdProvider>();

// Register the Swagger services
builder.Services.AddOpenApiDocument(document =>
        {
            document.Title = "BlazorApp1 API";
            document.Version = "v1";

            document.AddSecurity("JWT", new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<INotifier, Notifier>();
builder.Services.AddScoped<IItemsNotifier, ItemsNotifier>();

builder.Services.AddScoped<IFileUploaderService, FileUploaderService>();
builder.Services.AddScoped<IUrlHelper, UrlHelper>();

builder.Services.AddAzureClients(builder =>
{
    // Add a KeyVault client
    //builder.AddSecretClient(keyVaultUrl);

    // Add a Storage account client
    builder.AddBlobServiceClient(Configuration.GetConnectionString("Azure:Storage"))
                    .WithVersion(BlobClientOptions.ServiceVersion.V2019_07_07);

    // Use DefaultAzureCredential by default
    builder.UseCredential(new DefaultAzureCredential());
});

// Add the reverse proxy capability to the server
var proxyBuilder = builder.Services.AddReverseProxy();
// Initialize the reverse proxy from the "ReverseProxy" section of configuration
proxyBuilder.LoadFromConfig(Configuration.GetSection("ReverseProxy"));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
})
.AddMassTransitHostedService(true)
.AddGenericRequestClient();

// builder.Services.AddStackExchangeRedisCache(o =>
// {
//     o.Configuration = Configuration.GetConnectionString("redis");
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseOpenApi();
app.UseSwaggerUi3();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<TestHub>("/hubs/test");
app.MapHub<ItemsHub>("/hubs/items");

app.MapReverseProxy();

app.MapFallbackToFile("index.html");

await SeedData.EnsureSeedData(app);

var blobServiceClient = app.Services.GetRequiredService<BlobServiceClient>();
var client = blobServiceClient.GetBlobContainerClient("images");
await client.CreateIfNotExistsAsync();

app.Run();