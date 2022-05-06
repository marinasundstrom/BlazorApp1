using BlazorApp1.Domain.Events;
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
        await emailService.SendEmail("test@email.com", "Item created", "This is a <b>test</b>.");
    }
}
