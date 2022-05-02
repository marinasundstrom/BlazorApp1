﻿using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using BlazorApp1.Server.Models;
using BlazorApp1.Server.Services;

namespace BlazorApp1.Server.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        ICurrentUserService currentUserService, 
        IDateTimeService dateTime) : base(options, operationalStoreOptions)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

#nullable disable

    public DbSet<Item> Items { get; set; }

#nullable restore

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.Created = _dateTime.Now;
                entry.Entity.CreatedById = _currentUserService.UserId!;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = _dateTime.Now;
                entry.Entity.LastModifiedById = _currentUserService.UserId;
            }
            else if(entry.State == EntityState.Deleted 
                && entry.Entity is ISoftDelete e) 
            { 
                e.Deleted = _dateTime.Now;
                e.DeletedById = _currentUserService.UserId;

                entry.State = EntityState.Modified;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
