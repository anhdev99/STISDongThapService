using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetRankWithPagingDto : IMapFrom<Rank>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackGroundColor { get; set; }
    public string? Color { get; set; }
}

public class GetRankDto : IMapFrom<Rank>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackGroundColor { get; set; }
    public string? Color { get; set; }
}

public class RankSimpleDto : IMapFrom<Rank>
{
    public int Id { get; set; }
    public string Name { get; set; }
}