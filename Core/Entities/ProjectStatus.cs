using Shared;

namespace Core.Entities;

public class ProjectStatus : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
}