using Shared;

namespace Core.Entities;

public class Menu : BaseAuditableEntity
{
    public string Name { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsBlank { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public Menu? Parent { get; set; }
    
    public ICollection<Menu>? Children { get; set; }
    public ICollection<RoleMenu>? RoleMenus { get; set; }
}