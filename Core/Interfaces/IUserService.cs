using Core.DTOs.Responses;
using Shared;

namespace Core.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetProfileByUserName(string username);

}