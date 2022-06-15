namespace BlazorApp1.Domain.Events;

public class ItemStatusUpdated : DomainEvent
{
    public ItemStatusUpdated(string itemId, int statusId)
    {
        ItemId = itemId;
        StatusId = statusId;
    }

    public string ItemId { get; }

    public int StatusId { get; }
}