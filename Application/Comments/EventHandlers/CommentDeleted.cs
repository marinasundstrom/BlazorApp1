using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.EventHandlers;

public class CommentDeletedEventHandler : INotificationHandler<DomainEventNotification<CommentDeleted>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploaderService _fileUploaderService;
    private readonly IItemsNotifier _itemsNotifier;

    public CommentDeletedEventHandler(IApplicationDbContext context, IFileUploaderService fileUploaderService, IItemsNotifier itemsNotifier)
    {
        _context = context;
        _fileUploaderService = fileUploaderService;
        _itemsNotifier = itemsNotifier;
    }

    public async Task Handle(DomainEventNotification<CommentDeleted> notification, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .IgnoreQueryFilters()
            .FirstAsync(i => i.Id == notification.DomainEvent.CommentId, cancellationToken);
    }
}