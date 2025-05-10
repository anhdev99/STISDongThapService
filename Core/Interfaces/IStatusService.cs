using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IStatusService
{
    Task<Result<int>> Create(CreateStatusRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateStatusRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetStatusesWithPagingDto>> GetStatusesWithPaging(GetStatusesWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetStatusDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<StatusSimpleDto>>> GetAll();
}