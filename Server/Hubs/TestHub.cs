using MassTransit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Worker.Contracts;

namespace BlazorApp1.Server.Hubs;

[Authorize]
public class TestHub : Hub<ITestClient>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TestHub(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SayHi(string yourName) 
    {
        await _publishEndpoint.Publish(new WorkerMessage(yourName));

        // await Task.Delay(2000);

        //await Clients.All.Responded($"Greetings, {yourName}");
    }
}