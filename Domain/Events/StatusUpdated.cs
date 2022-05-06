namespace BlazorApp1.Domain.Events;

public class StatusUpdatedEvent : DomainEvent 
{
    public StatusUpdatedEvent(string itemId, int statusId) 
    {
        ItemId = itemId;
        StatusId = statusId;
    }

    public string ItemId { get; }

    public int StatusId { get; }
}
