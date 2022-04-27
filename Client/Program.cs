using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1.Client;
using MudBlazor.Services;
using System.Globalization;
using Microsoft.JSInterop;
using BlazorApp1.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("BlazorApp1.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorApp1.ServerAPI"));

builder.Services.AddMudServices();

builder.Services.AddLocalization();

builder.Services.AddApiAuthorization()
    .AddAccountClaimsPrincipalFactory<RolesClaimsPrincipalFactory>();

builder.Services.AddHttpClient<BlazorApp1.Client.IWeatherForecastClient>("BlazorApp1.ServerAPI")
    .AddTypedClient<IWeatherForecastClient>((http, sp) => new WeatherForecastClient(http));

builder.Services.AddHttpClient<BlazorApp1.Client.IItemsClient>("BlazorApp1.ServerAPI")
    .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http));

builder.Services.AddServices();

var app = builder.Build();

await Localize(app.Services);

await app.RunAsync();

static async Task Localize(IServiceProvider serviceProvider)
{
    CultureInfo culture;
    var js = serviceProvider.GetRequiredService<IJSRuntime>();
    var result = await js.InvokeAsync<string>("blazorCulture.get");

    if (result != null)
    {
        culture = new CultureInfo(result);
    }
    else
    {
        culture = new CultureInfo("en-US");
        await js.InvokeVoidAsync("blazorCulture.set", "en-US");
    }

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}