using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Entities;
using Shared;

namespace Core.Interfaces;

public interface IUserService
{
    Task<Result<int>> Create(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Update(int id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result<int>> Delete(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetUserWithPaginationDto>> GetUsersWithPagination(GetUsersWithPaginationQuery query,
        CancellationToken cancellationToken);

    Task<Result<GetUserDto>> GetById(int id, CancellationToken cancellationToken =default);
    Task<GetUserDto> GetById(int id);
    Task<Result<List<GetUserDto>>> GetAll(CancellationToken cancellationToken);

    Task<Result<bool>> Verify(string username, CancellationToken cancellationToken);
    Task<Result<bool>> ChangePassword(string username, string oldPassword, string newPassword, string confirmPassword, CancellationToken cancellationToken);
    Task<Result<string>> ResetPassword(string username, CancellationToken cancellationToken);
    Task<User?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Result<GetMeDto>> GetMe(CancellationToken cancellationToken);
}