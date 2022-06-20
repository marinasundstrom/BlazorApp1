
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Comments;

public static class ItemsDbSetExtensions
{
    public static IQueryable<Item> IncludeAll(this IQueryable<Item> source)
    {
        return source
            .Include(i => i.Status)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy);
        //.Include(i => i.DeletedBy)
    }

    public static IQueryable<Comment> IncludeAll(this IQueryable<Comment> source)
    {
        return source
            .Include(i => i.Item)
            .ThenInclude(i => i.Status)
            .Include(i => i.Item)
            .ThenInclude(i => i.CreatedBy)
            .Include(i => i.Item)
            .ThenInclude(i => i.LastModifiedBy)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy);
        //.Include(i => i.DeletedBy)
    }
}