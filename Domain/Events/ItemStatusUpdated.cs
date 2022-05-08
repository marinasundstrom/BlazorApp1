namespace BlazorApp1.Domain.Events;

public class ItemStatusUpdatedEvent : DomainEvent
{
    public ItemStatusUpdatedEvent(string itemId, int statusId)
    {
        ItemId = itemId;
        StatusId = statusId;
    }

    public string ItemId { get; }

    public int StatusId { get; }
}