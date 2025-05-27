namespace Core.DTOs.Requests;

public record CreatePositionRequest(string Code,string Name, int Order);

public record UpdatePositionRequest(int id, string Code,string Name, int Order);

public record GetPositionsWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);