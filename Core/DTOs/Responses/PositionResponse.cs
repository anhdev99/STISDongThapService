using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetPositionsWithPagingDto : IMapFrom<Position>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}

public class GetPositionDto : IMapFrom<Position>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}

public class PositionSimpleDto : IMapFrom<Position>
{
    public int Id { get; set; }
    public string Name { get; set; }
}