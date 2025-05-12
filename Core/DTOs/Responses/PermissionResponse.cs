using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetPermissionDto : IMapFrom<Permission>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool Priority{get;set;}
    public bool IsProtected{get;set;}
    public int Order { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Permission, GetPermissionDto>();
    }
}

public class GetAllPermissionsDto : IMapFrom<Permission>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool Priority{get;set;}
    public int Order { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Permission, GetAllPermissionsDto>();
    }
}

public class GetPermissionWithPaginationDto : IMapFrom<Permission>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool Priority{get;set;}
    public int Order { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Permission, GetPermissionWithPaginationDto>();
    }
}