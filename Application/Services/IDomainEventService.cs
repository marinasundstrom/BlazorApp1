using BlazorApp1.Domain;

namespace BlazorApp1.Application.Services;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}