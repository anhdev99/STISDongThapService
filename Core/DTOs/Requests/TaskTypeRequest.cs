namespace Core.DTOs.Requests;

public record CreateTaskTypeRequest(string Code,string Name, int Order);

public record UpdateTaskTypeRequest(string Code,string Name, int Order);

public record GetTaskTypesWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);