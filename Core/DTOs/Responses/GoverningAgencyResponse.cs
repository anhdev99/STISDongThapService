using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetGoverningAgencyWithPagingDto : IMapFrom<GoverningAgency>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public int Order { get; set; }
}

public class GetGoverningAgencyDto : IMapFrom<GoverningAgency>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public int Order { get; set; }
}

public class GoverningAgencySimpleDto : IMapFrom<GoverningAgency>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
