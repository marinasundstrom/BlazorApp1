using BlazorApp1.Application.Items;

namespace BlazorApp1.WebAPI.Hubs;

public interface IItemsClient
{
    Task ItemAdded(ItemDto item);

    Task ItemDeleted(string id, string name);

    Task ImageUploaded(string id, string image);
}