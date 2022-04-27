﻿namespace BlazorApp1.Client.Services;

public interface ICurrentUserService
{
    Task<string?> GetUserId();
    Task<bool> IsUserInRole(string role);
}
