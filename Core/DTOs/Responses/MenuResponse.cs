using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetMenuDto : IMapFrom<Menu>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsBlank { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public ICollection<GetMenuDto>? Children { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Menu, GetMenuDto>()
            .ForMember(dto => dto.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
            .ForMember(dto => dto.Children, opt => opt.MapFrom(src => src.Children));
    }
}
public class GetAllMenuDto : IMapFrom<Menu>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsBlank { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Menu, GetAllMenuDto>();
    }
}

public class GetMenuWithPaginationDto : IMapFrom<Menu>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsBlank { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Menu, GetMenuWithPaginationDto>();
    }
}
public class GetMenuTreeViewDto : IMapFrom<Menu>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public bool IsBlank { get; set; }
    public int? ParentId { get; set; }
    public ICollection<GetMenuTreeViewDto>? Children { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Menu, GetMenuTreeViewDto>();
    }
}
