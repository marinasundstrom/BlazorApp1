using BlazorApp1.Application.Comments;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain;

namespace BlazorApp1.Application.Items;

public static class Mapper
{
    public static ItemDto ToDto(this Item item, IUrlHelper urlHelper)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Status.ToDto(), urlHelper.CreateImageUrl(item.ImageId)!, item.Created, item.CreatedBy.UserName, item.LastModified, item.LastModifiedBy?.UserName);
    }

    public static StatusDto ToDto(this Status item)
    {
        return new StatusDto(item.Id, item.Name);
    }

    public static CommentDto ToDto(this Comment comment, IUrlHelper urlHelper)
    {
        return new CommentDto(comment.Id, comment.Item.ToDto(urlHelper), comment.Text, comment.Created, comment.CreatedBy.UserName, comment.LastModified, comment.LastModifiedBy?.UserName);
    }
}