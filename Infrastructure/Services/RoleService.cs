using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace Infrastructure.Services;

public class RoleService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<RoleService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger,dbContext, unitOfWork, mapper), IRoleService
{
    private readonly UserService _userService;
    private readonly ApplicationDbContext _context;

    public async Task<Result<int>> Create(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var existingCodeStatus = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Code == request.Code && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingCodeStatus != null)
        {
            throw new Exception("Mã vai trò đã tồn tại");
        }

        var name = request.Name.Trim();
        var existingNameRole = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Name.Equals(name) && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingNameRole != null)
        {
            throw new Exception("Tên vai trò đã tồn tại");
        }

        var entity = new Role
        {
            Name = name,
            Description = request.Description,
            Order = request.Order,
            Priority = request.Priority,
            Code = request.Code,
            DisplayName = request.DisplayName,
            Color = request.Color,
           
        };
        await _unitOfWork.Repository<Role>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        _logger.LogInformation($"Vai trò {request.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo vai trò thành công");
    }

    public async Task<Result<int>> Update(int id, UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("id:" + id);
        Console.WriteLine("name:" + request);
        var entity = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Vai trò không tồn tại");
        }
        
        if (request == null || string.IsNullOrWhiteSpace(request.Code))
        {
            throw new Exception("Thông tin yêu cầu không hợp lệ");
        }

        var existingStatus = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Code == request.Code && x.Id != request.id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingStatus != null)
        {
            throw new Exception("Mã vai trò đã tồn tại");
        }

        var name = request.Name.Trim();
        var existingRole = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Name.Equals(name) && x.Id != request.id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingRole != null)
        {
            throw new Exception("Tên vai trò đã tồn tại");
        }

        if (entity.IsProtected)
        {
            throw new Exception("Đang được bảo vệ");
        }

        entity.Name = name;
        entity.Description = request.Description;
        entity.Order = request.Order;
        entity.Priority = request.Priority;
        entity.DisplayName = request.DisplayName;
        entity.Code = request.Code;
        entity.Color = request.Color;

        await _unitOfWork.Repository<Role>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Vai trò {request.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật vai trò thành công");
    }
   



    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Vai trò không tồn tại");
        }

        if (entity.IsProtected)
        {
            throw new Exception("Đang được bảo vệ");
        }

        entity.IsDeleted = true;
        await _unitOfWork.Repository<Role>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Vai trò {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa vai trò thành công");
    }

    public async Task<PaginatedResult<GetRoleWithPaginationDto>> GetRolesWithPagination(
        GetRolesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Role>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(request.Keywords))
            query = query.Where(x => x.Name.ToLower().Trim().Contains(request.Keywords.ToLower().Trim()));

        return await query.OrderByDescending(x => x.Order)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetRoleWithPaginationDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }

    public async Task<Result<GetRoleDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<GetRoleDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Vai trò không tồn tại");
        }

        return await Result<GetRoleDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<GetRoleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.IsDeleted == false)
            .ProjectTo<GetRoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (data.Count <= 0)
        {
            throw new Exception("Không tìm thấy vai trò nào");
        }

        return await Result<List<GetRoleDto>>.SuccessAsync(data);
    }

    public async Task<Result<int>> AssignRoleMenus(RoleMenuRequest request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Id == request.RoleId && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
            throw new Exception("Vai trò không tồn tại");

        if (request.MenuIds != null && request.MenuIds.Any())
        {
            var existingRoleMenus = await _unitOfWork.Repository<RoleMenu>().Entities
                .Where(x => x.RoleId == request.RoleId).ToListAsync(cancellationToken);

            foreach (var item in existingRoleMenus)
            {
                item.IsDeleted = true;
                await _unitOfWork.Repository<RoleMenu>().UpdateAsync(item);
            }

            var existingMenus = await _unitOfWork.Repository<Menu>().Entities
                .Where(x => request.MenuIds.Contains(x.Id) && x.IsDeleted == false)
                .ToListAsync(cancellationToken);
            if (existingMenus.Count <= 0)
            {
                throw new Exception("Không tìm thấy menu nào");
            }

            if (existingMenus.Count != request.MenuIds.Count)
            {
                throw new Exception("Một số menu không tồn tại");
            }

            foreach (var menu in existingMenus)
            {
                var existingMenu = existingRoleMenus.FirstOrDefault(x => x.MenuId == menu.Id);
                if (existingMenu != null)
                {
                    existingMenu.IsDeleted = false;
                    await _unitOfWork.Repository<RoleMenu>().UpdateAsync(existingMenu);
                }
                else
                {
                    var newMenuTag = new RoleMenu { RoleId = request.RoleId, MenuId = menu.Id };
                    await _unitOfWork.Repository<RoleMenu>().AddAsync(newMenuTag);
                }
            }
        }
        else
        {
            throw new Exception("Menu Id là bắt buộc");
        }

        await _unitOfWork.Save(cancellationToken);
        return await Result<int>.SuccessAsync(role.Id, "Vai trò đã được gán menu");
    }

    public async Task<Result<int>> AssignRolePermissions(RolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Id == request.RoleId && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
            throw new Exception("Vai trò không tồn tại");

        if (request.PermissionIds != null && request.PermissionIds.Any())
        {
            var existingRolePermissions = await _unitOfWork.Repository<RolePermission>().Entities
                .Where(x => x.RoleId == request.RoleId).ToListAsync(cancellationToken);

            foreach (var item in existingRolePermissions)
            {
                item.IsDeleted = true;
                await _unitOfWork.Repository<RolePermission>().UpdateAsync(item);
            }

            var existingPermissions = await _unitOfWork.Repository<Permission>().Entities
                .Where(x => request.PermissionIds.Contains(x.Id) && x.IsDeleted == false)
                .ToListAsync(cancellationToken);
            if (existingPermissions.Count <= 0)
            {
                throw new Exception("Không tìm thấy quyền nào");
            }

            if (existingPermissions.Count != request.PermissionIds.Count)
            {
                throw new Exception("Một số quyền không tồn tại");
            }

            foreach (var permission in existingPermissions)
            {
                var existingPermission = existingRolePermissions.FirstOrDefault(x => x.PermissionId == permission.Id);
                if (existingPermission != null)
                {
                    existingPermission.IsDeleted = false;
                    await _unitOfWork.Repository<RolePermission>().UpdateAsync(existingPermission);
                }
                else
                {
                    var newMenuTag = new RolePermission { RoleId = request.RoleId, PermissionId = permission.Id };
                    await _unitOfWork.Repository<RolePermission>().AddAsync(newMenuTag);
                }
            }
        }
        else
        {
            throw new Exception("Danh sách quyền là bắt buộc");
        }

        await _unitOfWork.Save(cancellationToken);
        return await Result<int>.SuccessAsync(role.Id, "Vai trò đã được gán quyền");
    }

    public async Task<Result<List<int>>> GetConfigMenuByRoleId(int roleId, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>().Entities.Include(x => x.RoleMenus)
            .Where(x => x.Id == roleId  && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
        if (role == null)
            throw new Exception($"Không tìm thấy vai trò với Id {roleId}");

        var configMenuIds = role.RoleMenus.Where(x =>  !x.IsDeleted).Select(x => x.MenuId).ToList();

        return Result<List<int>>.Success(configMenuIds);
    }

    public async Task<Result<int>> ConfigUserRole(ConfigUserRoleRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("UserName: " + request.UserName);
        Console.WriteLine("RoleId: " + request.roleCode);
        Console.WriteLine("cancellationToken: " + cancellationToken);

        // Check for null user profile
        var userProfile = await _unitOfWork.Repository<User>().Entities
            .Where(x => x.UserName == request.UserName && x.IsDeleted != true)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (userProfile == null)
        {
            throw new Exception($"Không tìm thấy tài khoản {request.UserName}");
        }
        
        var userId = userProfile.Id;
        Console.WriteLine("userId: " + userId);
        
        // check for null role
        var role = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Code == request.roleCode && x.IsDeleted != true)
            .ProjectTo<GetRoleDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (role == null)
        {
            throw new Exception($"Không tìm thấy quyền {request.roleCode}");
        }

        var roleId = role.Id;
        Console.WriteLine("roleId: " + roleId);

        var rolePermission = await _unitOfWork.Repository<UserRole>().Entities
            .Where(x => x.UserId == userId && x.RoleId == roleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (rolePermission == null)
        {
            var newEntity = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };
            await _unitOfWork.Repository<UserRole>().AddAsync(newEntity);
            await _unitOfWork.Save(cancellationToken);
        }
        else
        {
            rolePermission.IsDeleted = !rolePermission.IsDeleted;
            await _unitOfWork.Repository<UserRole>().UpdateAsync(rolePermission);
            await _unitOfWork.Save(cancellationToken);
        }

        return await Result<int>.SuccessAsync(1, "Cấu hình vai trò người dùng thành công");
    }

    public async Task<Result<List<RolePermissionDto>>> GetPermissions(
        CancellationToken cancellationToken)
    {
        var permissions = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.IsDeleted != true).ToListAsync(cancellationToken);

        var result = permissions.GroupBy(x => x.Code.Split('.')[0]).Select(g => new RolePermissionDto
        {
            Group = g.Key,
            Permissions = g.Select(x => x.Code).ToList()
        }).ToList();

        return Result<List<RolePermissionDto>>.Success(result);
    }
    public async Task<Result<List<string>>> GetConfigPermissionByRoleId(int roleId,
        CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>().Entities.Include(x => x.RolePermissions)
            .Where(x => x.Id == roleId && x.IsDeleted != true).FirstOrDefaultAsync(cancellationToken);
        if (role == null)
            throw new Exception($"Không tìm thấy vai trò với Id {roleId}");

        var configPermissionIds = role.RolePermissions
            .Where(x => x.RoleId == role.Id && x.IsDeleted != true)
            .Select(x => x.PermissionId)
            .ToList();

        var permissions = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => configPermissionIds.Contains(x.Id) && x.IsDeleted != true)
            .ToListAsync(cancellationToken);

        var configPermissionNames = permissions.Select(x => x.Code).ToList();
        return await Result<List<string>>.SuccessAsync(configPermissionNames);
    }
    public async Task<Result<int>> ConfigPermissionRole(ConfigPermissionRoleRequest request,
        CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Repository<Role>().Entities
            .Where(x => x.Id == request.Id && x.IsDeleted != true).FirstOrDefaultAsync(cancellationToken);
        if (role == null)
            throw new Exception($"Không tìm thấy vai trò với Id {request.Id}");

        var existingRolePermissions = await _unitOfWork.Repository<RolePermission>().Entities
            .Where(x => x.RoleId == request.Id).ToListAsync(cancellationToken);

        foreach (var item in existingRolePermissions)
        {
            item.IsDeleted = true;
            await _unitOfWork.Repository<RolePermission>().UpdateAsync(item);
        }

        if (request.PermissionNames != null && request.PermissionNames.Any())
        {
            var existingPermissions = await _unitOfWork.Repository<Permission>().Entities
                .Where(x => request.PermissionNames.Contains(x.Code) && x.IsDeleted == false)
                .ToListAsync(cancellationToken);
            if (existingPermissions.Count <= 0)
            {
                throw new Exception("Không tìm thấy quyền nào");
            }

            if (existingPermissions.Count != request.PermissionNames.Count)
            {
                throw new Exception("Một số quyền không tồn tại");
            }

            foreach (var permission in existingPermissions)
            {
                var existingPermission = existingRolePermissions.FirstOrDefault(x => x.PermissionId == permission.Id);
                if (existingPermission != null)
                {
                    existingPermission.IsDeleted = false;
                    await _unitOfWork.Repository<RolePermission>().UpdateAsync(existingPermission);
                }
                else
                {
                    var newMenuTag = new RolePermission { RoleId = request.Id, PermissionId = permission.Id };
                    await _unitOfWork.Repository<RolePermission>().AddAsync(newMenuTag);
                }
            }
        }
        else
        {
            throw new Exception("Danh sách quyền là bắt buộc");
        }

        await _unitOfWork.Save(cancellationToken);
        return await Result<int>.SuccessAsync(role.Id, "Vai trò đã được gán quyền");
    }
    
    public List<string> GetRoleByProfileCode(int userId)
    {
        var roles =  _unitOfWork.Repository<UserRole>().Entities
            .Where(x => x.UserId == userId &&  x.IsDeleted != true).Select(x => x.Role.Code).ToList();
        
        return roles;
    }
}