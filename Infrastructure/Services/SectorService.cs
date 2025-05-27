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

public class SectorService (
    IHttpContextAccessor httpContextAccessor,
    ILogger<RoleService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger,dbContext, unitOfWork, mapper), ISectorService
{
    private readonly IMapper _mapper;
 
    
    public async Task<Result<int>> Create(CreateSectorRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _unitOfWork.Repository<Sector>().Entities
            .AnyAsync(x => x.Code == model.Code && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã ngành đã tồn tại");
        }
        
        var entity = new Sector()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
        };

        await _unitOfWork.Repository<Sector>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Ngành {model.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo ngành thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateSectorRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<Rank>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy ngành: {id}");
        }
        
        var existingStatus = await _unitOfWork.Repository<Rank>().Entities
            .AnyAsync(x => x.Code == model.Code && x.Id != id && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã ngành đã tồn tại");
        }


        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;
        
        
        await _unitOfWork.Repository<Rank>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Ngành {model.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật ngành thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<Rank>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy ngành: {id}");
        }

        entity.IsDeleted = true;

        await _unitOfWork.Repository<Rank>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Ngành {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa ngành thành công");
    }

    public async Task<PaginatedResult<GetSectorWithPagingDto>> GetSectorsWithPaging(GetSectorsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Sector>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(request.Keywords))
            query = query.Where(x => x.Name.ToLower().Trim().Contains(request.Keywords.ToLower().Trim()));

        return await query.OrderByDescending(x => x.Name)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetSectorWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetSectorDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Sector>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetSectorDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Ngành không tồn tại");
        }
        
        return await Result<GetSectorDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<SectorSimpleDto>>> GetAll()
    {
        var list = _unitOfWork.Repository<Sector>().Entities.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ProjectTo<SectorSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<SectorSimpleDto>>.SuccessAsync(list);
    }
}