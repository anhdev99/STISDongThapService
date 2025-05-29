using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetStatusesWithPagingDto : IMapFrom<ProjectStatus>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProjectStatus, GetStatusesWithPagingDto>();
    }
}

public class GetStatusDto : IMapFrom<ProjectStatus>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
}

public class StatusSimpleDto : IMapFrom<ProjectStatus>
{
    public int Id { get; set; }
    public string Name { get; set; }
}