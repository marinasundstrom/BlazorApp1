using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Domain;

public class Role : IdentityRole<string>
{
    public Role()
    {
        Id = Guid.NewGuid().ToString();
    }

    public List<User> Users { get; } = new List<User>();

    public List<UserRole> UserRoles { get; } = new List<UserRole>();
}