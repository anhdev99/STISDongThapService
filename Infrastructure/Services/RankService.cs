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

public class RankService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper) , IRankService
{
    private readonly IMapper _mapper;
   
    
    public async Task<Result<int>> Create(CreateRankRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _unitOfWork.Repository<Rank>().Entities
            .AnyAsync(x => x.Code == model.Code && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã cấp bật đã tồn tại");
        }
        
        var entity = new Rank()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
            BackgroundColor = model.BackgroundColor,
            Color = model.Color
        };

        await _unitOfWork.Repository<Rank>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Cấp bật {model.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo cấp bật thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateRankRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<Rank>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy cấp bậc: {id}");
        }
        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;
        entity.BackgroundColor = model.BackgroundColor;
        entity.Color = model.Color;

        var existingStatus = await _unitOfWork.Repository<Rank>().Entities
            .AnyAsync(x => x.Code == model.Code.Trim() &&  x.Id != id && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã cấp bậc đã tồn tại");
        }
        

        await _unitOfWork.Repository<Rank>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Cấp bật {model.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật Cấp bật thành công");
        
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<Rank>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Cấp bậc không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _unitOfWork.Repository<Rank>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Cấp bật {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa Cấp bật thành công");
    }

    public async Task<PaginatedResult<GetRankWithPagingDto>> GetRanksWithPaging(GetRanksWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<Rank>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
            filteredQuery = filteredQuery.Where(x => x.Name.ToLower().Trim().Contains(query.Keywords.ToLower().Trim()));

        return await filteredQuery.OrderByDescending(x => x.Name)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetRankWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetRankDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Project>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetRankDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Cấp bật không tồn tại");
        }

        return await Result<GetRankDto>.SuccessAsync(entity);
        
    }
    
    public async Task<Result<List<RankSimpleDto>>> GetAll()
    {
        var query = _unitOfWork.Repository<Project>()
            .Entities 
            .Where(x => !x.IsDeleted)
            .ProjectTo<RankSimpleDto>(_mapper.ConfigurationProvider);

        var list = await query.ToListAsync();

        return await Result<List<RankSimpleDto>>.SuccessAsync(list);
    }

}