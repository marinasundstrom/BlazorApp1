using System.IdentityModel.Tokens.Jwt;

using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Infrastructure.Persistence.Interceptors;
using BlazorApp1.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Infrastructure;

public static class ServiceExtensions
{
    private const string ConnectionStringKey = "sqlserver";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringKey, "BlazorApp1");

        services.AddScoped<MyInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
            options.AddInterceptors(sp.GetRequiredService<MyInterceptor>());
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        AddIdentity(services);

        services.AddScoped<IDateTimeService, DateTimeService>();

        services.AddScoped<IDomainEventService, DomainEventService>();

        return services;
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<User, ApplicationDbContext>(opt =>
            {
                // Is this necessary with a profile?

                opt.IdentityResources["openid"].UserClaims.Add("role");
                opt.ApiResources.Single().UserClaims.Add("role");
            });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");
    }
}