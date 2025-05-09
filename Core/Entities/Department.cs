using Shared;

namespace Core.Entities;

public class Department : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public Department Parent { get; set; }
    public ICollection<Department> Children { get; set; }
}