using System;

namespace BlazorApp1.Application;

public record PagedResult<T>(IEnumerable<T> Items, int Page, int TotalCount);