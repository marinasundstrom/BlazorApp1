
namespace BlazorApp1.Server.Models
{
    public interface ISoftDelete
    {
        DateTime? Deleted { get; set; }

        string? DeletedById { get; set; }

        ApplicationUser? DeletedBy { get; set; }
    }
}