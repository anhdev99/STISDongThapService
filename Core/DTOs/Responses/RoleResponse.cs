using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetRoleDto : IMapFrom<Role>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? DisplayName { get; set; }
    public string? Code { get; set; }
    public string Color { get; set; }

    public bool Priority {get; set;}
    public bool IsProtected{get; set;}

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Role, GetRoleDto>();
    }
}

public class GetAllRolesDto : IMapFrom<Role>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? DisplayName { get; set; }
    public string? Code { get; set; }
    public bool Priority{get; set;}


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Role, GetAllRolesDto>();
    }
}

public class GetRoleWithPaginationDto : IMapFrom<Role>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool Priority {get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Role, GetRoleWithPaginationDto>();
    }
}
public class RolePermissionDto
{
    public string Group { get; set; }
    public List<string> Permissions { get; set; }

}
public class BaseRole
{
    public string Code { get; set; }
    public string DisplayName { get; set; }
    public string Color { get; set; }
}