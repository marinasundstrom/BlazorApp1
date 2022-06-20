using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Common;
using BlazorApp1.Infrastructure.Persistence.Interceptors;

using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorApp1.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IApplicationDbContext, IPersistedGrantDbContext
{
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
    private readonly IDomainEventService _domainEventService;
    private readonly ITenantService _tenantService;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly string? _tenantId;

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IDomainEventService domainEventService,
        ITenantService tenantService,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _operationalStoreOptions = operationalStoreOptions;
        _domainEventService = domainEventService;
        _tenantService = tenantService;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _tenantId = _tenantService.TenantId;
    }

#nullable disable

    public DbSet<Item> Items { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Status> Statuses { get; set; }

    public DbSet<PersistedGrant> PersistedGrants { get; set; }

    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

    public DbSet<Key> Keys { get; set; }

#nullable restore

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ConfigTenantFilter(modelBuilder);
    }

    private void ConfigTenantFilter(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasQueryFilter(e => e.TenantId == _tenantId && e.Deleted == null);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasQueryFilter(e => e.TenantId == _tenantId && e.Deleted == null);
        });
    }

    Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _domainEventService.DispatchDomainEvents(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}