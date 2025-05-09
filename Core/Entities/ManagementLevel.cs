using Shared;

namespace Core.Entities;

public class ManagementLevel : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
}