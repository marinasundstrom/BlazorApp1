namespace BlazorApp1.Client.Authentication;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}