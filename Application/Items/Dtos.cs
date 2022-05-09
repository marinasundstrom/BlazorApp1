namespace BlazorApp1.Application.Items;

public record ItemDto(string Id, string Name, string Description, StatusDto Status, string? Image, DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);

public record StatusDto(int Id, string Name);

public record PagedResult<T>(IEnumerable<T> Items, int Page, int TotalCount);