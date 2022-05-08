using BlazorApp1.Application.Services;
using BlazorApp1.WebAPI.Hubs;

using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.WebAPI.Services;

public class Notifier : INotifier
{
    private readonly IHubContext<TestHub, ITestClient> _hubContext;

    public Notifier(IHubContext<TestHub, ITestClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Notify(string message)
    {
        await _hubContext.Clients.All.Responded(message);
    }
}