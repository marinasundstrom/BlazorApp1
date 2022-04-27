namespace BlazorApp1.Client.Services;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();

        return services;
    }
}