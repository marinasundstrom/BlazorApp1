using BlazorApp1.Domain.Events;

using Utils;

namespace BlazorApp1.Domain;

public class Item : AuditableEntity, ISoftDelete, IHasDomainEvent, IHasTenant
{
    private readonly HashSet<Comment> _comments = new HashSet<Comment>();

    private Item()
    {

    }

    public Item(string name, string description, int statusId)
    {
        Id = Guider.ToUrlFriendlyString(Guid.NewGuid());
        Name = name;
        Description = description;
        StatusId = statusId;

        DomainEvents.Add(new ItemCreated(Id));
    }

    public string Id { get; private set; }

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
        if(description != Description)
        {
            Description = description;

            OnUpdated();
        }
    }

    private void OnUpdated()
    {
        if(!DomainEvents.OfType<ItemUpdated>().Any())
        {
            DomainEvents.Add(new ItemUpdated(Id));
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

        DomainEvents.Add(new ItemStatusUpdated(Id, StatusId));
        OnUpdated();
    }

    public string? ImageId { get; set; }

    public IReadOnlyCollection<Comment> Comments => _comments;

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
        comment.DomainEvents.Add(new CommentCreated(comment.Id));
    } 

    public void RemoveComment(Comment comment)
    {
        _comments.Remove(comment);
        comment.DomainEvents.Add(new CommentDeleted(comment.Id));
    }

    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public User? DeletedBy { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}