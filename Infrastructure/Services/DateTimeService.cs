using System.Net.Mail;

using BlazorApp1.Application.Services;

namespace BlazorApp1.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}