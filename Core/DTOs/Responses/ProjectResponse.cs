using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetProjectWithPagingDto :IMapFrom<Project>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Content { get; init; }
    public string Target { get; init; }
    public int SectorId { get; init; }
    public int? StatusId { get; init; }
    public int? ManagementLevelId { get; init; }
    public int? TaskTypeId { get; init; }
    public int? ProjectLeaderId { get; init; }
    public int? HostOrganizationId { get; init; }
    public int? GoverningAgencyId { get; init; }
    public int? RankId { get; init; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Project, GetProjectWithPagingDto>();
    }
}

public record GetProjectDetailDto:IMapFrom<Project>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Content { get; init; }
    public string Target { get; init; }
    public int SectorId { get; init; }
    public int? StatusId { get; init; }
    public int? ManagementLevelId { get; init; }
    public int? TaskTypeId { get; init; }
    public int? ProjectLeaderId { get; init; }
    public int? HostOrganizationId { get; init; }
    public int? GoverningAgencyId { get; init; }
    public int? RankId { get; init; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Project, GetProjectDetailDto>();
    }
}