namespace BlazorApp1.Domain;

public class Status : IHasDomainEvents
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Item> Items { get; set; } = new List<Item>();

    /*
    public DateTime? Deleted { get; set; }

    public string? DeletedById { get; set; }

    public User? DeletedBy { get; set; } 
    */

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}