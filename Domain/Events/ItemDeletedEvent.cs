namespace BlazorApp1.Domain.Events;

public class ItemDeletedEvent : DomainEvent 
{
    public ItemDeletedEvent(string itemId) 
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}
