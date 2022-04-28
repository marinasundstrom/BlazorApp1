﻿using System.Security.Claims;

using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.Server;

public class EmailBasedUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Email)?.Value!;
    }
}
