namespace Shared.Interfaces;

public interface IAuditableEntity : IEntity
{
    string? CreatedBy { get; set; }
    DateTime? CreatedDate { get; set; }
    string? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}