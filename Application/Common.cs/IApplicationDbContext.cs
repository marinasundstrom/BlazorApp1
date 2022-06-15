
using BlazorApp1.Domain;

using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Common
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<Item> Items { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<Status> Statuses { get; set; }

        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}