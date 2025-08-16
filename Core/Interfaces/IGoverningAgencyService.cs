using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IGoverningAgencyService
{
    Task<Result<int>> Create(CreateGoverningAgencyRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateGoverningAgencyRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetGoverningAgencyWithPagingDto>> GetGoverningAgenciesWithPaging(GetGoverningAgenciesWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetGoverningAgencyDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<GoverningAgencySimpleDto>>> GetAll();
}
