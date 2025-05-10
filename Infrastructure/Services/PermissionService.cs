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

public class PermissionService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<PermissionService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IPermissionService
{
    private readonly ApplicationDbContext _context;

    public async Task<Result<int>> Create(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        
        var existingStatus = await _context.Permissions
            .AnyAsync(x => x.Code == request.Code, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã quyền đã tồn tại");
        }
        
        var name = request.Name.Trim();
        var existingPermission = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.Name.Equals(name) && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingPermission != null)
        {
            throw new Exception("Tên quyền đã tồn tại");
        }

        var entity = new Permission
        {
            Name = name,
            Description = request.Description,
            Order = request.Order,
            Code = request.Code,
            Priority = request.Priority,
            IsProtected = true,
        };
        await _unitOfWork.Repository<Permission>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        _logger.LogInformation($"Quyền {request.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo quyền thành công");
    }

    public async Task<Result<int>> Update(int id, UpdatePermissionRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Không tìm thấy quyền");
        }
        
        var existingStatus = await _context.Permissions
            .AnyAsync(x => x.Code == request.Code.Trim(), cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã vai trò đã tồn tại");
        }

        if (entity.IsProtected)
        {
            throw new Exception("Đang được bảo vệ");
        }

        var name = request.Name.Trim();
        var existingPermission = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.Name.Equals(name) && x.Id != id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingPermission != null)
        {
            throw new Exception("Tên quyền đã tồn tại");
        }

        entity.Name = name;
        entity.Description = request.Description;
        entity.Order = request.Order;
        entity.Code = request.Code;
        entity.Priority = request.Priority;

        await _unitOfWork.Repository<Permission>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Quyền {request.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật quyền thành công");
    }

    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Không tìm thấy quyền");
        }

        if (entity.IsProtected)
        {
            throw new Exception("Đang được bảo vệ");
        }

        entity.IsDeleted = true;
        await _unitOfWork.Repository<Permission>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Quyền {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa quyền thành công");
    }

    public async Task<PaginatedResult<GetPermissionWithPaginationDto>> GetPermissionsWithPagination(
        GetPermissionsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Permission>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(request.Keywords))
            query = query.Where(x => x.Name.ToLower().Trim().Contains(request.Keywords.ToLower().Trim()));

        if (request.RoleIds != null)
            query = query.Where(x =>
                x.RolePermissions != null && x.RolePermissions.Any(p => request.RoleIds.Contains(p.Id)));

        return await query.OrderByDescending(x => x.Order)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetPermissionWithPaginationDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }

    public async Task<Result<GetPermissionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<GetPermissionDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Không tìm thấy quyền");
        }

        return await Result<GetPermissionDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<GetPermissionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.Repository<Permission>().Entities
            .Where(x => x.IsDeleted == false)
            .ProjectTo<GetPermissionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (data.Count <= 0)
        {
            throw new Exception("Không tìm thấy quyền nào");
        }

        return await Result<List<GetPermissionDto>>.SuccessAsync(data);
    }
}
