namespace BlazorApp1.Application.Services;

public interface IUrlHelper
{
    string GetHostUrl();

    string? CreateImageUrl(string? id);
}