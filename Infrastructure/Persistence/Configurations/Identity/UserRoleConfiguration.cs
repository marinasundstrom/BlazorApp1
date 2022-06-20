namespace BlazorApp1.Infrastructure.Persistence.Configurations.Identity;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        //in case you chagned the TKey type
        //  entity.HasKey(key => new { key.UserId, key.RoleId });
    }
}
