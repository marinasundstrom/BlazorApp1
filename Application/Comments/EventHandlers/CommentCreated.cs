using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments.EventHandlers;

public class CommentCreatedEventHandler : INotificationHandler<DomainEventNotification<CommentCreated>>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IItemsNotifier _itemsNotifier;
    private readonly IUrlHelper _urlHelper;

    public CommentCreatedEventHandler(IApplicationDbContext context, IEmailService emailService, IItemsNotifier itemsNotifier, IUrlHelper urlHelper)
    {
        _context = context;
        _emailService = emailService;
        _itemsNotifier = itemsNotifier;
        _urlHelper = urlHelper;
    }

    public async Task Handle(DomainEventNotification<CommentCreated> notification, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .IgnoreQueryFilters()
            .FirstAsync(i => i.Id == notification.DomainEvent.CommentId, cancellationToken);
    }
}