using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemCreatedEventHandler : INotificationHandler<DomainEventNotification<ItemCreated>>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IItemsNotifier _itemsNotifier;
    private readonly IUrlHelper _urlHelper;

    public ItemCreatedEventHandler(IApplicationDbContext context, IEmailService emailService, IItemsNotifier itemsNotifier, IUrlHelper urlHelper)
    {
        _context = context;
        _emailService = emailService;
        _itemsNotifier = itemsNotifier;
        _urlHelper = urlHelper;
    }

    public async Task Handle(DomainEventNotification<ItemCreated> notification, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .IncludeAll()
            .IgnoreQueryFilters()
            .FirstAsync(i => i.Id == notification.DomainEvent.ItemId, cancellationToken);

        await _itemsNotifier.NotifyItemAdded(item.ToDto(_urlHelper));

        await _emailService.SendEmail("test@email.com", "Item created", "This is a <b>test</b>.");
    }
}