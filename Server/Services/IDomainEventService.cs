using BlazorApp1.Server.Models;

namespace BlazorApp1.Server.Services;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}