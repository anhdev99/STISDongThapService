using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IPositionService 
{
    Task<Result<int>> Create(CreatePositionRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdatePositionRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetPositionsWithPagingDto>> GetPositionsWithPaging(GetPositionsWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetPositionDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<PositionSimpleDto>>> GetAll();
}