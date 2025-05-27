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

public class PositionService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IPositionService
{
    public async Task<Result<int>> Create(CreatePositionRequest model, CancellationToken cancellationToken)
    {
        var isExisting = await _unitOfWork.Repository<Position>().Entities
            .AnyAsync(x => x.Code == model.Code.Trim() && !x.IsDeleted, cancellationToken);

        if (isExisting)
        {
            throw new Exception("Mã vị trí đã tồn tại");
        }

        var entity = new Position
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
            Priority = false
        };

        await _unitOfWork.Repository<Position>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Tạo chức vụ thành công!");
    }
    
    public async Task<Result<int>> Update(int id, UpdatePositionRequest model, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Position>().GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy chức vụ: {id}");
        }
        
        var existingStatus = await _unitOfWork.Repository<Position>().Entities
            .AnyAsync(x => x.Code == model.Code.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã chức vụ đã tồn tại");
        }
        
        entity.Code = model.Code.Trim();
        entity.Name = model.Name.Trim();
        entity.Order = model.Order;
        
        await _unitOfWork.Repository<Position>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật thông tin thành công!");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Position>().GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy chức vụ với Id là {id}");
        }

        await _unitOfWork.Repository<Position>().DeleteAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa chức vụ thành công");
    }

    public async Task<PaginatedResult<GetPositionsWithPagingDto>> GetPositionsWithPaging(GetPositionsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<Position>().Entities.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.OrderByDescending(x => x.Order)
            .ProjectTo<GetPositionsWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetPositionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Position>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetPositionDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetPositionDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<PositionSimpleDto>>> GetAll()
    {
        var list = await _unitOfWork.Repository<Position>().Entities.Where(x => !x.IsDeleted).OrderBy(x => x.Order)
            .ProjectTo<PositionSimpleDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return await Result<List<PositionSimpleDto>>.SuccessAsync(list);
    } 
}