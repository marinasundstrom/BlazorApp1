using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.Server;
using BlazorApp1.Server.Data;
using BlazorApp1.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using NSwag;
using NSwag.Generation.Processors.Security;
using Microsoft.AspNetCore.Identity;
using BlazorApp1.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("sqlserver");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(opt =>
    {
        opt.IdentityResources["openid"].UserClaims.Add("role");
        opt.ApiResources.Single().UserClaims.Add("role");
    });

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

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

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

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

        // builder.Services.AddMassTransit(x =>
        // {
        //     x.SetKebabCaseEndpointNameFormatter();
        // 
        //     x.AddConsumers(typeof(Program).Assembly);
        // 
        //     x.UsingRabbitMq((context, cfg) =>
        //     {
        //         cfg.ConfigureEndpoints(context);
        //     });
        // })
        // .AddMassTransitHostedService(true)
        // .AddGenericRequestClient();

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
app.MapFallbackToFile("index.html");

await SeedData.EnsureSeedData(app);

app.Run();
