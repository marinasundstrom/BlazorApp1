using BlazorApp1.Server.Models;

using MediatR;

namespace BlazorApp1.Server.Items.Events;

public class ItemCreatedEvent : DomainEvent 
{
    public ItemCreatedEvent(string itemId) 
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}

public class ItemUpdatedEvent : DomainEvent 
{
    public ItemUpdatedEvent(string itemId) 
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}

public class ItemDeletedEvent : DomainEvent 
{
    public ItemDeletedEvent(string itemId) 
    {
        ItemId = itemId;
    }

    public string ItemId { get; }
}