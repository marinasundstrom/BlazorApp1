using BlazorApp1.Application.Items;
using BlazorApp1.Application.Services;
using BlazorApp1.WebAPI.Hubs;

using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.WebAPI.Services;

public class ItemsNotifier : IItemsNotifier
{
    private readonly IHubContext<ItemsHub, Hubs.IItemsClient> _itemsHubContext;

    public ItemsNotifier(IHubContext<ItemsHub, Hubs.IItemsClient> itemsHubContext)
    {
        _itemsHubContext = itemsHubContext;
    }

    public async Task NotifyImageUploaded(string id, string image)
    {
        await _itemsHubContext.Clients.All.ImageUploaded(id, image);
    }

    public async Task NotifyItemAdded(ItemDto item)
    {
        await _itemsHubContext.Clients.All.ItemAdded(item);
    }

    public async Task NotifyItemDeleted(string id, string name)
    {
        await _itemsHubContext.Clients.All.ItemDeleted(id, name);
    }
}