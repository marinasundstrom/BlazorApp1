using MediatR;
using BlazorApp1.Server.Services;
using BlazorApp1.Server.Items.Events;

namespace BlazorApp1.Server.Items.EventHandlers;

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
