using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetSectorWithPagingDto : IMapFrom<Sector>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}

public class GetSectorDto : IMapFrom<Sector>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}

public class SectorSimpleDto : IMapFrom<Sector>
{
    public int Id { get; set; }
    public string Name { get; set; }
}