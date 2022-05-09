
using BlazorApp1.Application.Items;

namespace BlazorApp1.Application.Services;

public interface IItemsNotifier
{
    Task NotifyItemAdded(ItemDto item);

    Task NotifyItemDeleted(string id, string name);

    Task NotifyImageUploaded(string id, string image);
}