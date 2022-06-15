namespace BlazorApp1.Domain.Events;

public class ItemUpdated : DomainEvent
{
    public ItemUpdated(string itemId)
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}