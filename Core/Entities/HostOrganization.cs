using Shared;

namespace Core.Entities;

/// <summary>
///  Tổ chức chủ trì
/// </summary>
public class HostOrganization : BaseAuditableEntity
{
    public string Name { get; set; }
    public string? TaxCode { get; set; }
    public string? Representative { get; set; }
    public string? Position { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public int Order { get; set; }
}