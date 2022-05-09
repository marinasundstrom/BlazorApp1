namespace BlazorApp1.Application.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }

        bool IsUserInRole(string role);
    }
}