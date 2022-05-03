using BlazorApp1.WebAPI.Hubs;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.WebAPI.Consumers;

public class ResponseConsumer : IConsumer<Worker.Contracts.WorkerResponse>
{
    private readonly IHubContext<TestHub, ITestClient> _hubContext;

    public ResponseConsumer(IHubContext<TestHub, ITestClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<Worker.Contracts.WorkerResponse> context)
    {
        var response = context.Message;

        await _hubContext.Clients.All.Responded(response.Text);
    }
}
