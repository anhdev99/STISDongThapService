namespace Core.DTOs.Requests;

public record CreateHostOrganizationRequest(
    string Name,
    string? TaxCode,
    string? Representative,
    string? Position,
    string? Phone,
    string? Email,
    string? Address,
    string? Website,
    int Order
);

public record UpdateHostOrganizationRequest(
    int Id,
    string Name,
    string? TaxCode,
    string? Representative,
    string? Position,
    string? Phone,
    string? Email,
    string? Address,
    string? Website,
    int Order
);

public record GetHostOrganizationsWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Keywords
);
