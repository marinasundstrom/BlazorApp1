using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Domain.Entities;

public class UserRole : IdentityUserRole<string>
{
    public User User { get; set; } = null!;

    public Role Role { get; set; } = null!;
}