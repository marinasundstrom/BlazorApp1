
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemDeletedEventHandler : INotificationHandler<DomainEventNotification<ItemDeleted>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploaderService _fileUploaderService;
    private readonly IItemsNotifier _itemsNotifier;

    public ItemDeletedEventHandler(IApplicationDbContext context, IFileUploaderService fileUploaderService, IItemsNotifier itemsNotifier)
    {
        _context = context;
        _fileUploaderService = fileUploaderService;
        _itemsNotifier = itemsNotifier;
    }

    public async Task Handle(DomainEventNotification<ItemDeleted> notification, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .IgnoreQueryFilters()
            .FirstAsync(i => i.Id == notification.DomainEvent.ItemId, cancellationToken);

        if (item.ImageId is not null)
        {
            await _fileUploaderService.DeleteFileAsync(item.ImageId, cancellationToken);
        }

        await _itemsNotifier.NotifyItemDeleted(item.Id, item.Name);
    }
}