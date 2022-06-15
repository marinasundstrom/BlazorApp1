namespace BlazorApp1.Domain.Events;

public class CommentUpdated : DomainEvent
{
    public CommentUpdated(string commentId)
    {
        CommentId = commentId;
    }

    public string CommentId { get; }
}