
using Utils;

namespace BlazorApp1.Domain.Entities;

public class Item : BaseAuditableEntity, ISoftDelete, IHasTenant
{
    private readonly HashSet<Comment> _comments = new HashSet<Comment>();

    private Item()
    {

    }

    public Item(string name, string description, int statusId)
    {
        Name = name;
        Description = description;
        StatusId = statusId;

        AddDomainEvent(new ItemCreated(Id));
    }

    public string Id { get; private set; } = Guider.ToUrlFriendlyString(Guid.NewGuid());

    public string TenantId { get; set; } = null!;

    public string Name { get; private set; } = null!;

    public void SetName(string name)
    {
        if (name != Name)
        {
            Name = name;

            OnUpdated();
        }
    }

    public string Description { get; private set; } = null!;

    public void SetDescription(string description)
    {
        if (description != Description)
        {
            Description = description;

            OnUpdated();
        }
    }

    private void OnUpdated()
    {
        if (!DomainEvents.OfType<ItemUpdated>().Any())
        {
            AddDomainEvent(new ItemUpdated(Id));
        }
    }

    public Status Status { get; private set; } = null!;

    public int StatusId { get; private set; }

    public void SetStatus(Status newStatus)
    {
        Status = newStatus;

        SetStatus(Status.Id);
    }

    public void SetStatus(int newStatus)
    {
        StatusId = newStatus;

        AddDomainEvent(new ItemStatusUpdated(Id, StatusId));
        OnUpdated();
    }

    public string? ImageId { get; set; }

    public IReadOnlyCollection<Comment> Comments => _comments;

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
        AddDomainEvent(new CommentCreated(comment.Id));
    }

    public void RemoveComment(Comment comment)
    {
        _comments.Remove(comment);
        AddDomainEvent(new CommentDeleted(comment.Id));
    }

    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public User? DeletedBy { get; set; }
}