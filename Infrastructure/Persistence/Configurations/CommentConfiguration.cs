namespace BlazorApp1.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        // NOTE: Defined in ApplicationDbContext to allow for multi-tenancy.
        // builder.HasQueryFilter(i => i.Deleted == null);

        builder.HasIndex(nameof(Comment.TenantId));
    }
}