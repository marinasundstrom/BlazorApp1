using BlazorApp1.Application.Items;

namespace BlazorApp1.Application.Comments;

public record CommentDto(string Id, ItemDto Item, string Text, DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);
