namespace Core.DTOs.Requests;


public record CreateDepartmentRequest(string Code,string Name, int Order,int? ParentId);
public record UpdateDepartmentRequest(int Id,string Code,string Name, int Order,int? ParentId);
public record GetDepartmentsWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);