
using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class DepartmentResponse : IMapFrom<Department>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public string Label { get; set; }
    public string Value { get; set; }
    public List<DepartmentResponse>? Children { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Department, DepartmentResponse>().ForMember(x => x.Label, opt => opt.MapFrom(d => d.Name))
            .ForMember(x => x.Value, opt => opt.MapFrom(d => d.Id.ToString()));
    }
}

public class DepartmentTreeDto : IMapFrom<Department>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; } 
    public List<DepartmentTreeDto> Children { get; set; }
}

public class DepartmentDto : IMapFrom<Department>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; } 
}

public class GetDepartmentWithPagingDto : IMapFrom<Department>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; } 
}

public class GetDepartmentDto : IMapFrom<Department>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; } 
}