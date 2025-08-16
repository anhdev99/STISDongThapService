using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IManagementLevelService
{
    Task<Result<int>> Create(CreateManagementLevelRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateManagementLevelRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetManagementLevelWithPagingDto>> GetManagementLevelsWithPaging(GetManagementLevelsWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetManagementLevelDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<ManagementLevelSimpleDto>>> GetAll();
}