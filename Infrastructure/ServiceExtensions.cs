using System.IdentityModel.Tokens.Jwt;

using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Infrastructure.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Infrastructure;

public static class ServiceExtensions
{
    private const string ConnectionStringKey = "sqlserver";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringKey);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure()));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        AddIdentity(services);

        services.AddScoped<IDateTimeService, DateTimeService>();

        services.AddScoped<IDomainEventService, DomainEventService>();

        return services;
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(opt =>
            {
                opt.IdentityResources["openid"].UserClaims.Add("role");
                opt.ApiResources.Single().UserClaims.Add("role");
            });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");            
    }
}
