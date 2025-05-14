using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IProjectService
{
    Task<Result<int>> Create(CreateProjectRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateProjectRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);
    Task<PaginatedResult<GetProjectWithPagingDto>> GetProjectsWithPaging(GetProjectsWithPaginationQuery query, 
        CancellationToken cancellationToken);
    Task<Result<GetProjectDetailDto>> GetById(int id, CancellationToken cancellationToken);
}