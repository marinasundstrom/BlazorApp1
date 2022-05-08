namespace BlazorApp1.Domain.Events;

public class ItemCreatedEvent : DomainEvent
{
    public ItemCreatedEvent(string itemId)
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}