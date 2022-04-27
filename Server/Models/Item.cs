namespace BlazorApp1.Server.Models;

public class Item : AuditableEntity, ISoftDelete
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public ApplicationUser? DeletedBy { get; set; }
}
