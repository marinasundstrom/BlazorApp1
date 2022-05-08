namespace BlazorApp1.Domain.Events;

public class ItemUpdatedEvent : DomainEvent
{
    public ItemUpdatedEvent(string itemId)
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}