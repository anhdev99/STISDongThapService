using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IMenuService
{
    Task<Result<int>> Create(CreateMenuRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateMenuRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetMenuWithPaginationDto>> GetMenusWithPagination(GetMenusWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetMenuDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<GetMenuDto>>> GetAll(CancellationToken cancellationToken);
    Task<Result<List<GetMenuTreeViewDto>>> GetTreeView(CancellationToken cancellationToken);
    Task<Result<List<GetMenuTreeViewDto>>> GetMenusByUserRoles(CancellationToken cancellationToken);
}