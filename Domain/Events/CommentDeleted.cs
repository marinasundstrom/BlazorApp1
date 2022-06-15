namespace BlazorApp1.Domain.Events;

public class CommentDeleted : DomainEvent
{
    public CommentDeleted(string commentId)
    {
        CommentId = commentId;
    }

    public string CommentId { get; }
}
