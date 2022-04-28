namespace BlazorApp1.Client.Theming;

public static class ServicesExtensions
{
    public static IServiceCollection AddThemeServices(this IServiceCollection services)
    {
        services.AddScoped<ThemeDetector>();

        return services;
    }
}