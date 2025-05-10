using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface ISectorService
{
    Task<Result<int>> Create(CreateSectorRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateSectorRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetSectorWithPagingDto>> GetSectorsWithPaging(GetSectorsWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetSectorDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<SectorSimpleDto>>> GetAll();
}