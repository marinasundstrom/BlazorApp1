namespace BlazorApp1.Server.Items;

public record ItemDto(string Id, string Name, string Description, DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);

public record ItemsResult<T>(IEnumerable<T> Items, int TotalCount);