﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlazorApp1.Domain;

namespace BlazorApp1.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasQueryFilter(i => i.Deleted == null); 

        builder.Ignore(i => i.DomainEvents);   
    }
}