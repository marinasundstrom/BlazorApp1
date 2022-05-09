using System;

namespace BlazorApp1.Application.Services;

public interface INotifier
{
    Task Notify(string message);
}