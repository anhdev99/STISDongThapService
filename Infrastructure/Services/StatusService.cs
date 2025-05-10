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

public class StatusService : IStatusService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public StatusService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<int>> Create(CreateStatusRequest model, CancellationToken cancellationToken)
    {
        var entity = new Status()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
            BackgroundColor = model.BackgroundColor,
            Color = model.Color
        };

        _context.Statuss.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Tạo cấp bậc thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateStatusRequest model, CancellationToken cancellationToken)
    {
        var entity = _context.Statuss.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy cấp bậc: {id}");
        }
        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;
        entity.BackgroundColor = model.BackgroundColor;
        entity.Color = model.Color;

        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật cấp bậc thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _context.Statuss.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Cấp bậc không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa cấp bậc thành công");
    }

    public async Task<PaginatedResult<GetStatusWithPagingDto>> GetStatussWithPaging(GetStatussWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _context.Statuss.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.OrderByDescending(x => x.Order)
            .ProjectTo<GetStatusWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetStatusDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Statuss.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetStatusDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetStatusDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<StatusSimpleDto>>> GetAll()
    {
        var list = _context.Statuss.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ProjectTo<StatusSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<StatusSimpleDto>>.SuccessAsync(list);
    }
}