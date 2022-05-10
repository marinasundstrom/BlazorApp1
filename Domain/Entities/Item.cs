using BlazorApp1.Domain.Events;

namespace BlazorApp1.Domain;

public class Item : AuditableEntity, ISoftDelete, IHasDomainEvent, IHasTenant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string TenantId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Status Status { get; private set; } = null!;

    public int StatusId { get; set; }

    public void SetStatus(Status newStatus)
    {
        Status = newStatus;

        SetStatus(Status.Id);
    }

    public void SetStatus(int newStatus)
    {
        StatusId = newStatus;

        DomainEvents.Add(new ItemStatusUpdatedEvent(Id, StatusId));
    }

    public string? ImageId { get; set; }

    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public User? DeletedBy { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}