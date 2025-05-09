using Shared;

namespace Core.Entities;

public class UserNotification : BaseAuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public bool IsRead { get; set; }
    public bool IsArchived { get; set; }
    
    public int NotificationId { get; set; }
    public Notification Notification { get; set; } 
}