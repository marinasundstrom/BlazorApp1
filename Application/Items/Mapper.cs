using BlazorApp1.Domain;

namespace BlazorApp1.Application.Items;

public static class Mapper
{
    public static ItemDto ToDto(this Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Created, item.CreatedBy.UserName, item.LastModified, item.LastModifiedBy?.UserName);
    }
}
