﻿using BlazorApp1.Domain.Events;
using BlazorApp1.Application.Services;

using MediatR;

namespace BlazorApp1.Application.Items.EventHandlers;

public class ItemUpdatedEventHandler : INotificationHandler<DomainEventNotification<ItemUpdatedEvent>>
{
    public Task Handle(DomainEventNotification<ItemUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
