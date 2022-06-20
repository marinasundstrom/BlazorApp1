using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Domain.Entities;

public class User : IdentityUser
{
    public List<Role> Roles { get; set; } = new List<Role>();

    public List<UserRole> UserRoles { get; } = new List<UserRole>();
}