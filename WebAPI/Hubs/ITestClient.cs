namespace BlazorApp1.WebAPI.Hubs;

public interface ITestClient
{
    Task Responded(string message);
}