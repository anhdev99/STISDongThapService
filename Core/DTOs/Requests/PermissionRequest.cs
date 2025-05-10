using System.Runtime.CompilerServices;

namespace Core.DTOs.Requests;

public record CreatePermissionRequest(
    string Name,
    string Code,
    string Description,
    int Order,
    bool Priority
    );

public record UpdatePermissionRequest(
    string Code,
    string Name,
    string Description,
    int Order,
    bool Priority);

public record GetPermissionsWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Keywords,
    List<int>? RoleIds);