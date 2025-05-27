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

public class ProjectStatusService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper),  IStatusService
{
    private readonly IMapper _mapper;
    
    
    public async Task<Result<int>> Create(CreateStatusRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _unitOfWork.Repository<ProjectStatus>().Entities.AnyAsync(x => x.Code == model.Code && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã trạng thái đã tồn tại");
        }

        var entity = new ProjectStatus
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
        };

        await _unitOfWork.Repository<ProjectStatus>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Trạng thái dự án {model.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo trạng thái dự án thành công");
        
    }
    
    public async Task<Result<int>> Update(int id, UpdateStatusRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<ProjectStatus>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy trạng thái: {id}");
        }
        var existingStatus = await _unitOfWork.Repository<ProjectStatus>().Entities
            .AnyAsync(x => x.Code == model.Code.Trim() && x.Id != id, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã vai trò đã tồn tại");
        }
        
        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;
        
        await _unitOfWork.Repository<ProjectStatus>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Trạng thái dự án {model.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật trạng thái dự án thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<ProjectStatus>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Trạng thái không tìm thấy: {id}");
        }

        entity.IsDeleted = true;

        await _unitOfWork.Repository<ProjectStatus>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Trạng thái dự án {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa trạng thái dự án thành công");
    }

    public async Task<PaginatedResult<GetStatusesWithPagingDto>> GetStatusesWithPaging(GetStatusesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Project>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(request.Keywords))
            query = query.Where(x => x.Name.ToLower().Trim().Contains(request.Keywords.ToLower().Trim()));

        return await query.OrderByDescending(x => x.Name)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetStatusesWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetStatusDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<ProjectStatus>().Entities
            .Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetStatusDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Dự án không tồn tại");
        }

        return await Result<GetStatusDto>.SuccessAsync(entity);    
    }
    
    public async Task<Result<List<StatusSimpleDto>>> GetAll()
    {
        var list =  _unitOfWork.Repository<ProjectStatus>().Entities.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ProjectTo<StatusSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<StatusSimpleDto>>.SuccessAsync(list);
    }
}