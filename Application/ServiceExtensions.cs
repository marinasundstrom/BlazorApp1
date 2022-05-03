using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) 
    {
        services.AddMediatR(typeof(ServiceExtensions));

        return services;
    }
}
