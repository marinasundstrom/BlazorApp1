using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemImageUploadedEventHandler : INotificationHandler<DomainEventNotification<ItemImageUploadedEvent>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUrlHelper _urlHelper;
    private readonly IItemsNotifier _itemsNotifier;

    public ItemImageUploadedEventHandler(IApplicationDbContext context, IUrlHelper urlHelper, IItemsNotifier itemsNotifier)
    {
        _context = context;
        _urlHelper = urlHelper;
        _itemsNotifier = itemsNotifier;
    }

    public async Task Handle(DomainEventNotification<ItemImageUploadedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var item = await _context.Items
            .AsNoTracking()
            .FirstAsync(i => i.Id == domainEvent.ItemId, cancellationToken);

        await _itemsNotifier.NotifyImageUploaded(domainEvent.ItemId, _urlHelper.CreateImageUrl(item.ImageId)!);
    }
}