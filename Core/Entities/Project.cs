using Shared;

namespace Core.Entities;

public class Project : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}