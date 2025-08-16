using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetHostOrganizationWithPagingDto : IMapFrom<HostOrganization>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? TaxCode { get; set; }
    public string? Representative { get; set; }
    public string? Position { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public int Order { get; set; }
}

public class GetHostOrganizationDto : IMapFrom<HostOrganization>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? TaxCode { get; set; }
    public string? Representative { get; set; }
    public string? Position { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public int Order { get; set; }
}

public class HostOrganizationSimpleDto : IMapFrom<HostOrganization>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
