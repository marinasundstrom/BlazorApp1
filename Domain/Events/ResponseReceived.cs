using BlazorApp1.Domain;

namespace BlazorApp1.Domain.Events;

public class ResponseReceived : DomainEvent
{
    public ResponseReceived(string message)
    {
        Message = message;
    }

    public string Message { get; }
}