
using BlazorApp1.Domain;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Common
{
    public interface IApplicationDbContext
    {
        DbSet<Item> Items { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}