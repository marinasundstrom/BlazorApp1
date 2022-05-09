using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.WebAPI.Hubs;

[Authorize]
public class ItemsHub : Hub<IItemsClient>
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}