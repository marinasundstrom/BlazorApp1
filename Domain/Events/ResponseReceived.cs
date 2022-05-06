using BlazorApp1.Domain;

namespace BlazorApp1.Domain.Events;

public class ResponseReceivedEvent : DomainEvent
{
    public ResponseReceivedEvent(string message)
    {
        Message = message;
    }

    public string Message { get; }
}