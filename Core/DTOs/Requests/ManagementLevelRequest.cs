namespace Core.DTOs.Requests;

public record CreateManagementLevelRequest(string Code, string Name, int Order);

public record UpdateManagementLevelRequest(int Id, string Code, string Name, int Order);

public record GetManagementLevelsWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);
