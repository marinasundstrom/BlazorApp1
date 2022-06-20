namespace BlazorApp1.Infrastructure.Persistence.Configurations.Identity;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable("RoleClaims");
    }
}
