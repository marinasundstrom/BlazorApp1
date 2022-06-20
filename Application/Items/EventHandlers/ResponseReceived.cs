
using MediatR;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ResponseReceivedEventHandler : INotificationHandler<DomainEventNotification<ResponseReceived>>
{
    private readonly INotifier _notifier;

    public ResponseReceivedEventHandler(INotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task Handle(DomainEventNotification<ResponseReceived> notification, CancellationToken cancellationToken)
    {
        var message = notification.DomainEvent.Message;
        await _notifier.Notify(message);
    }
}