using BlazorApp1.Domain;

using MediatR;

namespace BlazorApp1.Application.Items.Events;

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

public class ResponseReceivedEvent : DomainEvent
{
    public ResponseReceivedEvent(string message)
    {
        Message = message;
    }

    public string Message { get; }
}