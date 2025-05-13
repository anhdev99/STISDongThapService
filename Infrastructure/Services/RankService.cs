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

public class RankService : IRankService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public RankService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<int>> Create(CreateRankRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _context.Ranks
            .AnyAsync(x => x.Code == model.Code, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã cấp bật đã tồn tại");
        }
        
        var entity = new Rank()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
            BackgroundColor = model.BackgroundColor,
            Color = model.Color
        };

        _context.Ranks.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Tạo cấp bậc thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateRankRequest model, CancellationToken cancellationToken)
    {
        var entity = _context.Ranks.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
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
        
        var existingStatus = await _context.Ranks
            .AnyAsync(x => x.Code == model.Code.Trim() &&  x.Id != id, cancellationToken);

        if (existingStatus)
        {
            return await Result<int>.FailureAsync("Mã cấp bậc đã tồn tại");
        }
        

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật cấp bậc thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _context.Ranks.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Cấp bậc không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa cấp bậc thành công");
    }

    public async Task<PaginatedResult<GetRankWithPagingDto>> GetRanksWithPaging(GetRanksWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _context.Ranks.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.OrderByDescending(x => x.Order)
            .ProjectTo<GetRankWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetRankDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Ranks.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetRankDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetRankDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<RankSimpleDto>>> GetAll()
    {
        var list = _context.Ranks.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ProjectTo<RankSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<RankSimpleDto>>.SuccessAsync(list);
    }
}