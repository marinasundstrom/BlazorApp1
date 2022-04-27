using BlazorApp1.Server.Models;

namespace BlazorApp1.Server.Items;

public static class Mapper
{
    public static ItemDto ToDto(this Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Created, item.CreatedBy.UserName, item.LastModified, item.LastModifiedBy?.UserName);
    }
}
