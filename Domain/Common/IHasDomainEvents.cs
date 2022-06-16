
namespace BlazorApp1.Domain;

public interface IHasDomainEvents
{
    public List<DomainEvent> DomainEvents { get; set; }
}