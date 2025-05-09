using Shared;

namespace Core.Entities;

public class UserRole : BaseAuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public bool IsDefault { get; set; }
}