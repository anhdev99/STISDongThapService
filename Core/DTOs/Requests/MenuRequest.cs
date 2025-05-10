namespace Core.DTOs.Requests;

public record CreateMenuRequest(
    string Name,
    string? Url,
    string? Description,
    string? Icon,
    bool IsBlank,
    int Order,
    int? ParentId);

public record UpdateMenuRequest(
    string Name,
    string? Url,
    string? Description,
    string? Icon,
    bool IsBlank,
    int Order,
    int? ParentId);

public record GetMenusWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Keywords,
    List<int>? RoleIds,
    int? ParentId);
