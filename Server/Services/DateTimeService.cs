using System.Net.Mail;

namespace BlazorApp1.Server.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}