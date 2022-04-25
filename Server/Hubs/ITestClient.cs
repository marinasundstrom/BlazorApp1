namespace BlazorApp1.Server.Hubs;

public interface ITestClient
{
    Task Responded(string message);
}