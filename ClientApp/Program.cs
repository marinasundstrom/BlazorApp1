using System.Globalization;

using BlazorApp1.Client;
using BlazorApp1.Client.Authentication;
using BlazorApp1.Client.Theming;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddLocalization();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthenticationServices();

builder.Services.AddThemeServices();

builder.Services.AddApiAuthorization()
    .AddAccountClaimsPrincipalFactory<RolesClaimsPrincipalFactory>();

builder.Services.AddHttpClient<BlazorApp1.Client.IWeatherForecastClient>("WebAPI")
    .AddTypedClient<IWeatherForecastClient>((http, sp) => new WeatherForecastClient(http));

builder.Services.AddHttpClient<BlazorApp1.Client.IItemsClient>("WebAPI")
    .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http));

builder.Services.AddHttpClient<BlazorApp1.Client.IStatusesClient>("WebAPI")
    .AddTypedClient<IStatusesClient>((http, sp) => new StatusesClient(http));

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