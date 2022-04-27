using MediatR;

namespace BlazorApp1.Server.Items.Events;

public record ItemCreatedEvent(string ItemId) : INotification;

public record ItemUpdatedEvent(string ItemId) : INotification;

public record ItemDeletedEvent(string ItemId) : INotification;