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

public class ProjectStatusService : IStatusService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public ProjectStatusService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<int>> Create(CreateStatusRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _context.Status
            .AnyAsync(x => x.Code == model.Code, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã trạng thái đã tồn tại");
        }

        var entity = new ProjectStatus
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
        };

        _context.Status.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Tạo trạng thái thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateStatusRequest model, CancellationToken cancellationToken)
    {
        var entity = _context.Status.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy trạng thái: {id}");
        }
        var existingStatus = await _context.Status
            .AnyAsync(x => x.Code == model.Code.Trim() && x.Id != id, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã vai trò đã tồn tại");
        }
        
        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;
        
        

        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật trạng thái thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _context.Status.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Trạng thái không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa trạng thái thành công");
    }

    public async Task<PaginatedResult<GetStatusesWithPagingDto>> GetStatusesWithPaging(GetStatusesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _context.Status.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.OrderByDescending(x => x.Order)
            .ProjectTo<GetStatusesWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetStatusDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Status.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetStatusDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetStatusDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<StatusSimpleDto>>> GetAll()
    {
        var list = _context.Status.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ProjectTo<StatusSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<StatusSimpleDto>>.SuccessAsync(list);
    }
}