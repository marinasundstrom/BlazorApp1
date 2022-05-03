namespace BlazorApp1.Client.Authentication;

public interface ICurrentUserService
{
    Task<string?> GetUserId();
    Task<bool> IsUserInRole(string role);
}
