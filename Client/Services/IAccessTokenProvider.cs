namespace BlazorApp1.Client.Services;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}
