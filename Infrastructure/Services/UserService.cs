using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Common;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Extensions;
using Shared.Helpers;

namespace Infrastructure.Services;

public class UserService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper) 
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IUserService
{
    public async Task<Result<int>> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var name = request.UserName.Trim();
        var existingUser = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.UserName.Equals(name) && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingUser != null)
        {
            throw new Exception("Tài khoản đã tồn tại");
        }
        
        if (request.Password != request.ConfirmPassword)
        {
            throw new Exception("Mật khẩu không trùng khớp");
        }

        var role = await _unitOfWork.Repository<Role>().Entities.Where(x => x.Code == RoleConstants.USER && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (role == null)
            throw new Exception("Không tìm thấy vai trò!");
        
        byte[] passwordHash, passwordSalt;
        PasswordHelper.GeneratePasswordHash(request.Password, out passwordHash, out passwordSalt);
        
        
        var entity = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            LastName = request.LastName,
            FirstName = request.FirstName,
            PhoneNumber = request.PhoneNumber,
            IsDeleted = false,
            DepartmentId = request.DepartmentId,
            PositionId = request.PositionId,
        };

        entity.PasswordHash = passwordHash;
        entity.PasswordSalt = passwordSalt;

        await _unitOfWork.Repository<User>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        
        var userRole = new UserRole
        {
            UserId = entity.Id,
            RoleId = role.Id
        };
        await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
        await _unitOfWork.Save(cancellationToken);
        entity.UserRoles.Add(userRole);
        
        
        _logger.LogInformation($"Tài khoản {request.UserName} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo tài khoản thành công");
    }

    public async Task<Result<int>> Update(int id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Không tìm thấy tài khoản");
        }

        var name = request.UserName.Trim();
        var existingUser = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.UserName.ToLower() == name.ToLower() && x.Id != id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            throw new Exception("Tài khoản đã tồn tại");
        }

        entity.UserName = request.UserName;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.PhoneNumber = request.PhoneNumber;
        entity.Email = request.Email;
        entity.DepartmentId = request.DepartmentId;
        entity.PositionId = request.PositionId;

        await _unitOfWork.Repository<User>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Tài khoản {request.UserName} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật tài khoản thành công");
    }

    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Không tìm thấy tài khoản");
        }

        entity.IsDeleted = true;
        await _unitOfWork.Repository<User>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Tài khoản {entity.UserName} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa tài khoản thành công");
    }

    public async Task<PaginatedResult<GetUserWithPaginationDto>> GetUsersWithPagination(
        GetUsersWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<User>().Entities
            .Include(u => u.UserRoles.Where(ur => !ur.IsDeleted))
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
        {
            var keyword = query.Keywords.Trim().ToLower();
            filteredQuery = filteredQuery.Where(x => x.UserName.ToLower().Contains(keyword));
        }

        if (query.DepartmentId.HasValue)
        {
            filteredQuery = filteredQuery.Where(x => x.DepartmentId == query.DepartmentId.Value);
        }

        if (query.RoleId.HasValue)
        {
            filteredQuery = filteredQuery.Where(x => x.UserRoles.Any(ur => ur.RoleId == query.RoleId));
        }

        return await filteredQuery
            .ProjectTo<GetUserWithPaginationDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }

    public async Task<Result<GetUserDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Không tìm thấy tài khoản");
        }

        return await Result<GetUserDto>.SuccessAsync(entity);
    }

    public async Task<GetUserDto> GetById(int id)
    {
        var entity = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new Exception("Không tìm thấy tài khoản");
        }
        return entity;
    }

    public async Task<Result<List<GetUserDto>>> GetAll(CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.IsDeleted == false)
            .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (data.Count <= 0)
        {
            throw new Exception("Không tìm thấy tài khoản nào");
        }

        return await Result<List<GetUserDto>>.SuccessAsync(data);
    }
    
    public async Task<Result<bool>> Verify(string username,  CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.UserName.ToLower() == username.ToLower() && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new Exception("Tài khoản không tồn tại");
        
        user.IsVerified = true;
        
        await _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Tài khoản {user.UserName} đã được xác thực");
        return await Result<bool>.SuccessAsync("Xác thực tài khoản thành công");
    } 
    public async Task<Result<bool>> ChangePassword(string username, string oldPassword, string newPassword, string confirmPassword, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.UserName.ToLower() == username.ToLower() && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new Exception("Tài khoản không tồn tại");

        if (!PasswordHelper.VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Mật khẩu cũ không đúng");

        if (newPassword != confirmPassword)
            throw new Exception("Mật khẩu mới không trùng khớp");

        PasswordHelper.GeneratePasswordHash(newPassword, out byte[] newHash, out byte[] newSalt);
        user.PasswordHash = newHash;
        user.PasswordSalt = newSalt;

        await _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.Save(cancellationToken);

        return await Result<bool>.SuccessAsync(true, "Đổi mật khẩu thành công");
    }

    public async Task<Result<string>> ResetPassword(string username, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.UserName.ToLower() == username.ToLower() && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new Exception("Tài khoản không tồn tại");

        var newPassword = PasswordHelper.GenerateRandomPassword(8);

        PasswordHelper.GeneratePasswordHash(newPassword, out byte[] hash, out byte[] salt);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        await _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Tài khoản {username} đã được reset mật khẩu");

        return await Result<string>.SuccessAsync(newPassword, "Reset mật khẩu thành công");
    }

    public async Task<User?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<User>().Entities.Where(x => x.UserName.ToLower() == userName.ToLower() && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Result<GetMeDto>> GetMe(CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().Entities.Where(x => x.UserName == UserName && x.IsDeleted == false)
            .ProjectTo<GetMeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        if(user == null)
            throw new Exception("Không tìm thấy tài khoản");
        
        return await Result<GetMeDto>.SuccessAsync(user);
    }
}