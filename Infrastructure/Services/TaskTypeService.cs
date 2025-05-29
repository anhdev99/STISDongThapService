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

public class TaskTypeService  (
    IHttpContextAccessor httpContextAccessor,
    ILogger<RoleService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger,dbContext, unitOfWork, mapper),  ITaskTypeService
{
    public async Task<Result<int>> Create(CreateTaskTypeRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _unitOfWork.Repository<TaskType>().Entities
            .AnyAsync(x => x.Code == model.Code && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã loại nhiệm vụ đã tồn tại");
        }
        
        var entity = new TaskType()
        {
            Code = model.Code,
            Name = model.Name,
        };

        await _unitOfWork.Repository<TaskType>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Loại nhiệm vụ {model.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo loại nhiệm vụ thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateTaskTypeRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<TaskType>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy loại nhiệm vụ: {id}");
        }
        entity.Code = model.Code;
        entity.Name = model.Name;

        
        var existingStatus = await _unitOfWork.Repository<TaskType>().Entities
            .AnyAsync(x => x.Code == model.Code.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã loại nhiệm vụ đã tồn tại");
        }
        
        await _unitOfWork.Repository<TaskType>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Loại nhiệm vụ {model.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật loại nhiệm vụ thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<TaskType>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Loại nhiệm vụ không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _unitOfWork.Repository<TaskType>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Loại nhiệm vụ {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa loại nhiệm vụ thành công");
    }

    public async Task<PaginatedResult<GetTaskTypeWithPagingDto>> GetTaskTypesWithPaging(GetTaskTypesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<TaskType>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
            filteredQuery = filteredQuery.Where(x => x.Name.ToLower().Trim().Contains(query.Keywords.ToLower().Trim()));

        return await filteredQuery.OrderByDescending(x => x.Name)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetTaskTypeWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetTaskTypeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<TaskType>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetTaskTypeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Loại nhiệm vụ không tồn tại");
        }
        
        return await Result<GetTaskTypeDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<TaskTypeSimpleDto>>> GetAll()
    {
        var list = _unitOfWork.Repository<TaskType>().Entities.Where(x => !x.IsDeleted).ProjectTo<TaskTypeSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<TaskTypeSimpleDto>>.SuccessAsync(list);
    }
}