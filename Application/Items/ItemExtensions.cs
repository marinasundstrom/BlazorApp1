using BlazorApp1.Domain;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Items;

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
}