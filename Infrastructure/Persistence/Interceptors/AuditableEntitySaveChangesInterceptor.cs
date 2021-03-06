using BlazorApp1.Application.Services;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlazorApp1.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;
    private readonly ITenantService _tenantService;

    public AuditableEntitySaveChangesInterceptor(
        ICurrentUserService currentUserService,
        IDateTimeService dateTime,
        ITenantService tenantService)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _tenantService = tenantService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
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
            else if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
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
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}