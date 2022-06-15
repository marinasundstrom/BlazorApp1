namespace BlazorApp1.Domain.Events;

public class CommentCreated : DomainEvent
{
    public CommentCreated(string commentId)
    {
        CommentId = commentId;
    }

    public string CommentId { get; }
}
