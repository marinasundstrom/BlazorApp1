namespace BlazorApp1.Server.Services;

public interface IEmailService
{
    Task SendEmail(string recipient, string subject, string body);
}
