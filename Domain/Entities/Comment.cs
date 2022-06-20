
using Utils;

namespace BlazorApp1.Domain.Entities;

public class Comment : BaseAuditableEntity, ISoftDelete, IHasTenant
{
    private Comment()
    {

    }

    public Comment(string text)
    {
        Text = text;

        AddDomainEvent(new CommentCreated(Id));
    }

    public string Id { get; private set; } = Guider.ToUrlFriendlyString(Guid.NewGuid());

    public string TenantId { get; set; } = null!;

    public Item Item { get; set; } = null!;

    public string Text { get; private set; } = null!;

    public void UpdateText(string text)
    {
        if (text != Text)
        {
            Text = text;

            OnUpdated();
        }
    }

    private void OnUpdated()
    {
        if (!DomainEvents.OfType<CommentUpdated>().Any())
        {
            AddDomainEvent(new CommentUpdated(Id));
        }
    }

    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public User? DeletedBy { get; set; }

}