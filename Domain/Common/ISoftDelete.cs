
namespace BlazorApp1.Domain;

public interface ISoftDelete
{
    DateTime? Deleted { get; set; }

    string? DeletedById { get; set; }

    User? DeletedBy { get; set; }
}