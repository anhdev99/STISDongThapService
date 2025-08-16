using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IHostOrganizationService
{
    Task<Result<int>> Create(CreateHostOrganizationRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateHostOrganizationRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetHostOrganizationWithPagingDto>> GetHostOrganizationsWithPaging(GetHostOrganizationsWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetHostOrganizationDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<HostOrganizationSimpleDto>>> GetAll();
}
