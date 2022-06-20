namespace BlazorApp1.Infrastructure.Persistence.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("Statuses");

        builder.HasMany(e => e.Items).WithOne(e => e.Status).OnDelete(DeleteBehavior.NoAction);
    }
}