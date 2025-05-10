using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IRankService
{
    Task<Result<int>> Create(CreateRankRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateRankRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetRankWithPagingDto>> GetRanksWithPaging(GetRanksWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetRankDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<RankSimpleDto>>> GetAll();
}