namespace Core.DTOs.Requests;

public record CreateTaskTypeRequest(string Code,string Name);

public record UpdateTaskTypeRequest(int Id,string Code,string Name);

public record GetTaskTypesWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);