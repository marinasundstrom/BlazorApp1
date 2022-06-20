namespace BlazorApp1.Infrastructure.Persistence.Configurations.Identity;

public class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("UserLogins");
        //in case you chagned the TKey type
        //  entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });     
    }
}
