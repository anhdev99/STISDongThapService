using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface ITaskTypeService
{
    Task<Result<int>> Create(CreateTaskTypeRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateTaskTypeRequest model, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetTaskTypeWithPagingDto>> GetTaskTypesWithPaging(GetTaskTypesWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetTaskTypeDto>> GetById(int id, CancellationToken cancellationToken);
    Task<Result<List<TaskTypeSimpleDto>>> GetAll();
}