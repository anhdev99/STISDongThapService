using Shared;

namespace Core.Entities;

public class Notification : BaseAuditableEntity
{
    public string Key { get; set; } = Guid.NewGuid().ToString();
    public string Module { get; set; } = "FaceId";
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public int RelatedTaskId { get; set; }
    public string Channel { get; set; }
    public int Priority { get; set; }
    public ICollection<UserNotification> UserNotifications { get; set; }
}