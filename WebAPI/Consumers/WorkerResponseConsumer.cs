using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;

using MassTransit;

using Worker.Contracts;

namespace BlazorApp1.WebAPI.Consumers;

public class WorkerResponseConsumer : IConsumer<WorkerResponse>
{
    private readonly IDomainEventService _domainEventService;

    public WorkerResponseConsumer(IDomainEventService domainEventService)
    {
        _domainEventService = domainEventService;
    }

    public async Task Consume(ConsumeContext<WorkerResponse> context)
    {
        var response = context.Message;

        var ev = new ResponseReceived(response.Text);
        ev.IsPublished = true;

        await _domainEventService.Publish(ev);
    }
}