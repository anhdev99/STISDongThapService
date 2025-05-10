using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetStatusWithPagingDto : IMapFrom<Status>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
}

public class GetStatusDto : IMapFrom<Status>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
}

public class StatusSimpleDto : IMapFrom<Status>
{
    public int Id { get; set; }
    public string Name { get; set; }
}