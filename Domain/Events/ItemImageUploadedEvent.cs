namespace BlazorApp1.Domain.Events;

public class ItemImageUploadedEvent : DomainEvent
{
    public ItemImageUploadedEvent(string itemId, string imageUri)
    {
        ItemId = itemId;
        ImageUri = imageUri;
    }

    public string ItemId { get; }
    public string ImageUri { get; }
}