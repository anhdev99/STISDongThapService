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

public class ManagementLevelService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IManagementLevelService
{
    public async Task<Result<int>> Create(CreateManagementLevelRequest model, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.Repository<ManagementLevel>().Entities
            .AnyAsync(x => x.Code == model.Code && !x.IsDeleted, cancellationToken);

        if (exists)
            throw new Exception("Mã cấp quản lý đã tồn tại");

        var entity = new ManagementLevel
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order
        };

        await _unitOfWork.Repository<ManagementLevel>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"ManagementLevel {model.Name} created");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo cấp quản lý thành công");
    }

    public async Task<Result<int>> Update(int id, UpdateManagementLevelRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<ManagementLevel>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
            throw new Exception($"Không tìm thấy cấp quản lý: {id}");

        var exists = await _unitOfWork.Repository<ManagementLevel>().Entities
            .AnyAsync(x => x.Code == model.Code.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);
        if (exists)
            throw new Exception("Mã cấp quản lý đã tồn tại");

        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;

        await _unitOfWork.Repository<ManagementLevel>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"ManagementLevel {model.Name} updated");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật cấp quản lý thành công");
    }

    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<ManagementLevel>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
            throw new Exception($"Cấp quản lý không tìm thấy: {id}");

        entity.IsDeleted = true;
        await _unitOfWork.Repository<ManagementLevel>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"ManagementLevel {entity.Name} deleted");
        return await Result<int>.SuccessAsync(id, "Xóa cấp quản lý thành công");
    }

    public async Task<PaginatedResult<GetManagementLevelWithPagingDto>> GetManagementLevelsWithPaging(GetManagementLevelsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<ManagementLevel>().Entities.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
            filteredQuery = filteredQuery.Where(x => x.Name.ToLower().Trim().Contains(query.Keywords.ToLower().Trim()));

        return await filteredQuery.OrderByDescending(x => x.Name)
            .ProjectTo<GetManagementLevelWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }

    public async Task<Result<GetManagementLevelDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<ManagementLevel>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetManagementLevelDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new Exception("Cấp quản lý không tồn tại");

        return await Result<GetManagementLevelDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<ManagementLevelSimpleDto>>> GetAll()
    {
        var query = _unitOfWork.Repository<ManagementLevel>()
            .Entities
            .Where(x => !x.IsDeleted)
            .ProjectTo<ManagementLevelSimpleDto>(_mapper.ConfigurationProvider);

        var list = await query.ToListAsync();

        return await Result<List<ManagementLevelSimpleDto>>.SuccessAsync(list);
    }
}