using Shared;

namespace Core.Entities;

public class RoleMenu : BaseAuditableEntity
{
    public int RoleId { get; set; }
    public int MenuId { get; set; }
    public Role Role { get; set; }
    public Menu Menu { get; set; }
}