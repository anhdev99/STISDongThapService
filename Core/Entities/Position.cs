using Shared;

namespace Core.Entities;

public class Position : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public bool Priority { get; set; }
}