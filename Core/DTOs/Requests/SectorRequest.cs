namespace Core.DTOs.Requests;

public record CreateSectorRequest(string Code,string Name, int Order);

public record UpdateSectorRequest(int Id,string Code,string Name, int Order);

public record GetSectorsWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);