using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;
using BlazorApp1.Infrastructure.Persistence.Configurations;

using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorApp1.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IApplicationDbContext, IPersistedGrantDbContext
{
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
    private readonly IDomainEventService _domainEventService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;
    private readonly ITenantService _tenantService;
    private readonly string? _tenantId;

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IDomainEventService domainEventService,
        ICurrentUserService currentUserService,
        IDateTimeService dateTime,
        ITenantService tenantService) : base(options)
    {
        _operationalStoreOptions = operationalStoreOptions;
        _domainEventService = domainEventService;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _tenantService = tenantService;

        _tenantId = _tenantService.TenantId;
    }

#nullable disable

    public DbSet<Item> Items { get; set; }

    public DbSet<Status> Statuses { get; set; }

    public DbSet<PersistedGrant> PersistedGrants { get; set; }

    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

    public DbSet<Key> Keys { get; set; }

#nullable restore

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ConfigTenantFilter(modelBuilder);

        ConfigIdentityModel(modelBuilder);
    }

    private void ConfigTenantFilter(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasQueryFilter(e => e.TenantId == _tenantId && e.Deleted == null);
        });
    }

    private void ConfigIdentityModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(name: "Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable(name: "Roles");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            //in case you chagned the TKey type
            //  entity.HasKey(key => new { key.UserId, key.RoleId });
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
            //in case you chagned the TKey type
            //  entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");

        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
            //in case you chagned the TKey type
            // entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

        });
    }

    Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = _dateTime.Now;
                entry.Entity.CreatedById = _currentUserService.UserId!;

                if (entry.Entity is IHasTenant e)
                {
                    e.TenantId = _tenantService.TenantId!;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = _dateTime.Now;
                entry.Entity.LastModifiedById = _currentUserService.UserId;
            }
            else if (entry.State == EntityState.Deleted
                && entry.Entity is ISoftDelete e)
            {
                e.Deleted = _dateTime.Now;
                e.DeletedById = _currentUserService.UserId;

                entry.State = EntityState.Modified;
            }
        }

        DomainEvent[] events = GetDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    private DomainEvent[] GetDomainEvents()
    {
        return ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}