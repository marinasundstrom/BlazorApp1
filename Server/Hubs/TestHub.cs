using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.Server.Hubs;

[Authorize]
public class TestHub : Hub<ITestClient>
{
    public async Task SayHi(string yourName) 
    {
        await Task.Delay(2000);
        
        await Clients.All.Responded($"Greetings, {yourName}");
    }
}