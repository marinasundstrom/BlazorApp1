
using MediatR;

namespace BlazorApp1.Application.Comments.EventHandlers;

public class CommentUpdatedEventHandler : INotificationHandler<DomainEventNotification<CommentUpdated>>
{
    public Task Handle(DomainEventNotification<CommentUpdated> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}