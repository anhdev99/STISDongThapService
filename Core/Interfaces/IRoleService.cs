using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IRoleService
{
    Task<Result<int>> Create(CreateRoleRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateRoleRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetRoleWithPaginationDto>> GetRolesWithPagination(GetRolesWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetRoleDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<GetRoleDto>>> GetAll(CancellationToken cancellationToken);
    Task<Result<int>> AssignRoleMenus(RoleMenuRequest request, CancellationToken cancellationToken);
    Task<Result<int>> AssignRolePermissions(RolePermissionRequest request, CancellationToken cancellationToken);
    Task<Result<List<int>>> GetConfigMenuByRoleId(int roleId, CancellationToken cancellationToken);

    Task<Result<int>> ConfigUserRole(ConfigUserRoleRequest request,
        CancellationToken cancellationToken);
    Task<Result<List<string>>> GetConfigPermissionByRoleId(int roleId, CancellationToken cancellationToken);
    Task<Result<List<RolePermissionDto>>> GetPermissions(CancellationToken cancellationToken);
    Task<Result<int>> ConfigPermissionRole(ConfigPermissionRoleRequest request, CancellationToken cancellationToken);
    List<string> GetRoleByProfileCode(int userId);
}