namespace Core.DTOs.Requests;

public record CreateStatusRequest(string Code,string Name, int Order,string? BackgroundColor, string? Color);

public record UpdateStatusRequest(string Code,string Name, int Order,string? BackgroundColor, string? Color);

public record GetStatusesWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);