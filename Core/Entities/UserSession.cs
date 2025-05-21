using Shared;

namespace Core.Entities;

public class UserSession : BaseAuditableEntity
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string DeviceType { get; set; }
}