using Shared;

namespace Core.Entities;

public class Sector : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
}