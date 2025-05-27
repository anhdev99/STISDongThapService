using Shared;

namespace Core.Entities;

public class User : BaseAuditableEntity
{
    public string UserName { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsVerified { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int? PositionId {get; set;}
    public Position? Position { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}