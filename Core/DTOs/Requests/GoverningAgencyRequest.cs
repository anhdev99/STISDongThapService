namespace Core.DTOs.Requests;

public record CreateGoverningAgencyRequest(string Name, string? ContactPerson, string? Phone, string? Email, string? Address, string? Website, int Order);

public record UpdateGoverningAgencyRequest(int Id, string Name, string? ContactPerson, string? Phone, string? Email, string? Address, string? Website, int Order);

public record GetGoverningAgenciesWithPaginationQuery(int PageNumber, int PageSize, string? Keywords);
