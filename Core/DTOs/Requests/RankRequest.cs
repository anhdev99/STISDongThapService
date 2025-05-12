namespace Core.DTOs.Requests;

public record CreateRankRequest(string Code,string Name, int Order,string? BackgroundColor, string? Color);

public record UpdateRankRequest(int id,string Code,string Name, int Order,string? BackgroundColor, string? Color);

public record GetRanksWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);