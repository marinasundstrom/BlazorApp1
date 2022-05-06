using BlazorApp1.Domain.Events;
using BlazorApp1.Application.Services;

using MediatR;
using BlazorApp1.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.EventHandlers;

public class StatusUpdatedEventHandler : INotificationHandler<DomainEventNotification<StatusUpdatedEvent>>
{
    private readonly IApplicationDbContext applicationDbContext;
    private readonly IEmailService emailService;

    public StatusUpdatedEventHandler(IApplicationDbContext applicationDbContext, IEmailService emailService)
    {
        this.applicationDbContext = applicationDbContext;
        this.emailService = emailService;
    }

    public async Task Handle(DomainEventNotification<StatusUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        var item = await applicationDbContext.Items.FirstAsync(i => i.Id == notification.DomainEvent.ItemId);

        var status = await applicationDbContext.Statuses.FirstAsync(s => s.Id == notification.DomainEvent.StatusId);

        await emailService.SendEmail("test@email.com", "Status changed", $"<h1>{item.Name}</h1>\n\nStatus changed to {status.Name}.");
    }
}
