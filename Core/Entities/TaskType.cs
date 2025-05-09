using Shared;

namespace Core.Entities;

public class TaskType : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
}