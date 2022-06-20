
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemStatusUpdatedEventHandler : INotificationHandler<DomainEventNotification<ItemStatusUpdated>>
{
    private readonly IApplicationDbContext context;
    private readonly IEmailService emailService;

    public ItemStatusUpdatedEventHandler(IApplicationDbContext context, IEmailService emailService)
    {
        this.context = context;
        this.emailService = emailService;
    }

    public async Task Handle(DomainEventNotification<ItemStatusUpdated> notification, CancellationToken cancellationToken)
    {
        var item = await context.Items.FirstAsync(i => i.Id == notification.DomainEvent.ItemId, cancellationToken);

        var status = await context.Statuses.FirstAsync(s => s.Id == notification.DomainEvent.StatusId, cancellationToken);

        await emailService.SendEmail("test@email.com", "Status changed", $"<h1>{item.Name}</h1>\n\nStatus changed to {status.Name}.");
    }
}