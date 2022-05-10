using System.Data.Common;

using BlazorApp1.Application.Services;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlazorApp1.Infrastructure.Persistence;

public class MyInterceptor : DbCommandInterceptor
{
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        //command.CommandText = $"USE DiscriminatorDB {command.CommandText}";

        return base.ReaderExecutingAsync(command, eventData, result);
    }
}