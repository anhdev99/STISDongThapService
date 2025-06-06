using Shared;

namespace Core.Entities;

/// <summary>
///  Cơ quan chủ quản
/// </summary>
public class GoverningAgency : BaseAuditableEntity
{
    public string Name { get; set; } 
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public int Order { get; set; }
}