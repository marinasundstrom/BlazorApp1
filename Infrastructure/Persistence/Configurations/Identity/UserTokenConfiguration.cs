namespace BlazorApp1.Infrastructure.Persistence.Configurations.Identity;

public class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("UserTokens");
        //in case you chagned the TKey type
        // entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
    }
}