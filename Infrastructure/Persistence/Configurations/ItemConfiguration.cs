namespace BlazorApp1.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        // NOTE: Defined in ApplicationDbContext to allow for multi-tenancy.
        // builder.HasQueryFilter(i => i.Deleted == null);

        builder.HasIndex(nameof(Item.TenantId));
    }
}
