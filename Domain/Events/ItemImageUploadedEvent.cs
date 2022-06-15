namespace BlazorApp1.Domain.Events;

public class ItemImageUploaded : DomainEvent
{
    public ItemImageUploaded(string itemId, string imageUri)
    {
        ItemId = itemId;
        ImageUri = imageUri;
    }

    public string ItemId { get; }
    public string ImageUri { get; }
}