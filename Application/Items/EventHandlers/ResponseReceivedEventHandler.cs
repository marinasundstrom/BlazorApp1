using BlazorApp1.Domain.Events;
using BlazorApp1.Application.Services;

using MediatR;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ResponseReceivedEventHandler : INotificationHandler<DomainEventNotification<ResponseReceivedEvent>>
{
    private readonly INotifier _notifier;

    public ResponseReceivedEventHandler(INotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task Handle(DomainEventNotification<ResponseReceivedEvent> notification, CancellationToken cancellationToken)
    {
        var message = notification.DomainEvent.Message;
        await _notifier.Notify(message);
    }
}
