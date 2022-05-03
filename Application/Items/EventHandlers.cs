using BlazorApp1.Application.Common;
using BlazorApp1.Application.Items.Events;
using BlazorApp1.Application.Services;

using MediatR;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemCreatedEventHandler : INotificationHandler<DomainEventNotification<ItemCreatedEvent>>
{
    private readonly IEmailService emailService;

    public ItemCreatedEventHandler(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public async Task Handle(DomainEventNotification<ItemCreatedEvent> notification, CancellationToken cancellationToken)
    {
        await emailService.SendEmail("test@email.com", "Test recipient", "This is a <b>test</b>.");
    }
}

public class ItemUpdatedEventHandler : INotificationHandler<DomainEventNotification<ItemUpdatedEvent>>
{
    public Task Handle(DomainEventNotification<ItemUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class ItemDeletedEventHandler : INotificationHandler<DomainEventNotification<ItemDeletedEvent>>
{
    public Task Handle(DomainEventNotification<ItemDeletedEvent> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

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
