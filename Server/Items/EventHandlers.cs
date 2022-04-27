using MediatR;
using BlazorApp1.Server.Services;
using BlazorApp1.Server.Items.Events;

namespace BlazorApp1.Server.Items.EventHandlers;

public class ItemCreatedEventHandler : INotificationHandler<ItemCreatedEvent>
{
    private readonly IEmailService emailService;

    public ItemCreatedEventHandler(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public async Task Handle(ItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        await emailService.SendEmail("test@email.com", "Test recipient", "This is a <b>test</b>.");
    }
}

public class ItemUpdatedEventHandler : INotificationHandler<ItemUpdatedEvent>
{
    public Task Handle(ItemUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class ItemDeletedEventHandler : INotificationHandler<ItemDeletedEvent>
{
    public Task Handle(ItemDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
