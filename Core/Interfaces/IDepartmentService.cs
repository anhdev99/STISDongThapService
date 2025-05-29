using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentTreeDto>> GetDepartments(string departmentCode,
            CancellationToken cancellationToken = default);  
        Task<Result<int>> Create(CreateDepartmentRequest model, CancellationToken cancellationToken);
        Task<Result<int>> Update(int id, UpdateDepartmentRequest model, CancellationToken cancellationToken);
        Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

        Task<PaginatedResult<GetDepartmentWithPagingDto>> GetDepartmentsWithPaging(GetDepartmentsWithPaginationQuery query,
            CancellationToken cancellationToken);
        Task<Result<GetDepartmentDto>> GetById(int id, CancellationToken cancellationToken);
        Task<Result<List<GetDepartmentDto>>> GetAllDepartment( CancellationToken cancellationToken);

        Task<Result<List<DepartmentResponse>>> GetFullDepartmentTree(CancellationToken cancellationToken);
    }
}