namespace Core.DTOs.Requests;

public record CreateRoleRequest(
    string Name,
    string Description,
    string DisplayName,
    int Order,
    string Code,
    bool Priority);

public record UpdateRoleRequest(
    int id,
    string Name,
    string Description,
    string DisplayName,
    int Order,
    string Code,
    bool Priority);

public record GetRolesWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Keywords);
public record RoleMenuRequest(
    int RoleId,
    List<int> MenuIds);
public record RolePermissionRequest(
    int RoleId,
    List<int> PermissionIds);

public record ConfigUserRoleRequest 
(
    string UserName,
    int RoleId
);
public record ConfigPermissionRoleRequest
(
   int Id,
   List<string> PermissionNames
);
