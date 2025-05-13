using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Extensions;

namespace Infrastructure.Services;

public class TaskTypeService : ITaskTypeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public TaskTypeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<int>> Create(CreateTaskTypeRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _context.TaskTypes
            .AnyAsync(x => x.Code == model.Code, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã loại nhiệm vụ đã tồn tại");
        }
        
        var entity = new TaskType()
        {
            Code = model.Code,
            Name = model.Name,
        };

        _context.TaskTypes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Tạo loại nhiệm vụ thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateTaskTypeRequest model, CancellationToken cancellationToken)
    {
        var entity = _context.TaskTypes.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy loại nhiệm vụ: {id}");
        }
        entity.Code = model.Code;
        entity.Name = model.Name;

        await _context.SaveChangesAsync(cancellationToken);
        
        var existingStatus = await _context.TaskTypes
            .AnyAsync(x => x.Code == model.Code.Trim() && x.Id != id, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã loại nhiệm vụ đã tồn tại");
        }
        

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật loại nhiệm vụ thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _context.TaskTypes.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Loại nhiệm vụ không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa loại nhiệm vụ thành công");
    }

    public async Task<PaginatedResult<GetTaskTypeWithPagingDto>> GetTaskTypesWithPaging(GetTaskTypesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _context.TaskTypes.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.ProjectTo<GetTaskTypeWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetTaskTypeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.TaskTypes.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetTaskTypeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetTaskTypeDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<TaskTypeSimpleDto>>> GetAll()
    {
        var list = _context.TaskTypes.Where(x => !x.IsDeleted).ProjectTo<TaskTypeSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<TaskTypeSimpleDto>>.SuccessAsync(list);
    }
}