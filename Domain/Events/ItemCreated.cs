namespace BlazorApp1.Domain.Events;

public class ItemCreated : DomainEvent
{
    public ItemCreated(string itemId)
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}
