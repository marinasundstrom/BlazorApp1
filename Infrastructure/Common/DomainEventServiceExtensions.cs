
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Infrastructure.Common;

public static class DomainEventServiceExtensions
{
    public static async Task DispatchDomainEvents(this IDomainEventService domainEventService, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await domainEventService.Publish(domainEvent);
    }
}