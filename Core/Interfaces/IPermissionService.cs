using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IPermissionService
{
    
    Task<Result<int>> Create(CreatePermissionRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdatePermissionRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetPermissionWithPaginationDto>> GetPermissionsWithPagination(GetPermissionsWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetPermissionDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<GetPermissionDto>>> GetAll(CancellationToken cancellationToken);
}