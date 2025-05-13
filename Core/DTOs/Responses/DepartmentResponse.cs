
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
    public List<DepartmentResponse>? Children { get; set; }
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