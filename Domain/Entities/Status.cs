namespace BlazorApp1.Domain.Entities;

public class Status : BaseAuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Item> Items { get; set; } = new List<Item>();
}