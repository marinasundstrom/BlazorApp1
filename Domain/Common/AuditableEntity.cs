namespace BlazorApp1.Domain
{
    public class AuditableEntity
    {
        public DateTime Created { get; set; }
        public string CreatedById { get; set; } = null!;
        public ApplicationUser CreatedBy { get; set; } = null!;

        public DateTime? LastModified { get; set; }
        public string? LastModifiedById { get; set; }
        public ApplicationUser? LastModifiedBy { get; set; }
    }
}