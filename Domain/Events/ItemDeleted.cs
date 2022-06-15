namespace BlazorApp1.Domain.Events;

public class ItemDeleted : DomainEvent
{
    public ItemDeleted(string itemId)
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}