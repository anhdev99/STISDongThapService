using Shared;

namespace Core.Entities;

public class Permission : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public bool Priority { get; set; }
    public bool IsProtected { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}