namespace BlazorApp1.Domain;

public class Status : IHasDomainEvent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Item> Items { get; set; } = new List<Item>();

    /*
    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public ApplicationUser? DeletedBy { get; set; } 
    */

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}