using Shared;

namespace Core.Entities;

public class Role : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public bool Priority { get; set; }
    public bool IsProtected { get; set; }
    public string? Color { get; set; }
    
    public ICollection<RolePermission> RolePermissions { get; set; }
    public ICollection<RoleMenu> RoleMenus { get; set; }
}