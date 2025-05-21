using AutoMapper;
using Core.Authorization;
using Core.DTOs;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Helpers;

namespace Infrastructure.Services;

public class IdentityService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper, IUserService userService, IJwtUtils jwtUtils)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IIdentityService
{
    public async Task<AuthenticationResponse> LoginAsync(LoginRequest command, string ipAddress, string userAgent,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByUserNameAsync(command.UserName.ToUpper().Trim(), cancellationToken);
        if (user == null)
            throw new ValidationCustomException(nameof(command.UserName), "Không tìm thấy tài khoản.");

        if (!PasswordHelper.VerifyPasswordHash(command.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new ValidationCustomException(nameof(command.Password), "Mật khẩu không chính xác.");
        }

        var userDto = mapper.Map<UserDto>(user);
        var accessToken = jwtUtils.GenerateJwtToken(userDto);
        var refreshToken = jwtUtils.GenerateRefreshToken(userDto, ipAddress);
        await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);

        var userSession = new UserSession()
        {
            UserId = user.Id,
            UserName = user.UserName,
            Token = accessToken.Item1,
            ExpiresAt = accessToken.Item2,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            DeviceType = userAgent.GetDeviceType()
        };
        await _unitOfWork.Repository<UserSession>().AddAsync(userSession);
        await _unitOfWork.Save(cancellationToken);
        
        var userAuth = new AuthenticatedUserResponse()
        {
            UserId = userDto.Id,
            UserName = userDto.UserName,
            FullName = userDto.FullName,
            Status = user.IsVerified,
        };

        return new AuthenticationResponse()
        {
            AccessToken = accessToken.Item1,
            RefreshToken = refreshToken.Token,
            User = userAuth,
        };
    }
}