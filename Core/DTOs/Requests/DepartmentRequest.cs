namespace Core.DTOs.Requests;


public record CreateDepartmentRequest(string Code,string Name, int Order,int? parentId);

public record UpdateDepartmentRequest(int id,string Code,string Name, int Order,int? parentId);
public record GetDepartmentsWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);