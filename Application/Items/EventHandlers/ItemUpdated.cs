
using MediatR;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemUpdatedEventHandler : INotificationHandler<DomainEventNotification<ItemUpdated>>
{
    public Task Handle(DomainEventNotification<ItemUpdated> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}