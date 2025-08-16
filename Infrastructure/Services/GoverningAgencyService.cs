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

public class GoverningAgencyService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IGoverningAgencyService
{
    public async Task<Result<int>> Create(CreateGoverningAgencyRequest model, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.Repository<GoverningAgency>().Entities
            .AnyAsync(x => x.Name == model.Name && !x.IsDeleted, cancellationToken);

        if (exists)
            throw new Exception("Tên cơ quan chủ quản đã tồn tại");

        var entity = new GoverningAgency
        {
            Name = model.Name,
            ContactPerson = model.ContactPerson,
            Phone = model.Phone,
            Email = model.Email,
            Address = model.Address,
            Website = model.Website,
            Order = model.Order
        };

        await _unitOfWork.Repository<GoverningAgency>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"GoverningAgency {model.Name} created");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo cơ quan chủ quản thành công");
    }

    public async Task<Result<int>> Update(int id, UpdateGoverningAgencyRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<GoverningAgency>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
            throw new Exception($"Không tìm thấy cơ quan chủ quản: {id}");

        var exists = await _unitOfWork.Repository<GoverningAgency>().Entities
            .AnyAsync(x => x.Name == model.Name.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);
        if (exists)
            throw new Exception("Tên cơ quan chủ quản đã tồn tại");

        entity.Name = model.Name;
        entity.ContactPerson = model.ContactPerson;
        entity.Phone = model.Phone;
        entity.Email = model.Email;
        entity.Address = model.Address;
        entity.Website = model.Website;
        entity.Order = model.Order;

        await _unitOfWork.Repository<GoverningAgency>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"GoverningAgency {model.Name} updated");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật cơ quan chủ quản thành công");
    }

    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<GoverningAgency>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
            throw new Exception($"Cơ quan chủ quản không tìm thấy: {id}");

        entity.IsDeleted = true;
        await _unitOfWork.Repository<GoverningAgency>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"GoverningAgency {entity.Name} deleted");
        return await Result<int>.SuccessAsync(id, "Xóa cơ quan chủ quản thành công");
    }

    public async Task<PaginatedResult<GetGoverningAgencyWithPagingDto>> GetGoverningAgenciesWithPaging(GetGoverningAgenciesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<GoverningAgency>().Entities.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
            filteredQuery = filteredQuery.Where(x => x.Name.ToLower().Trim().Contains(query.Keywords.ToLower().Trim()));

        return await filteredQuery.OrderByDescending(x => x.Name)
            .ProjectTo<GetGoverningAgencyWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }

    public async Task<Result<GetGoverningAgencyDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<GoverningAgency>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetGoverningAgencyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new Exception("Cơ quan chủ quản không tồn tại");

        return await Result<GetGoverningAgencyDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<GoverningAgencySimpleDto>>> GetAll()
    {
        var query = _unitOfWork.Repository<GoverningAgency>()
            .Entities
            .Where(x => !x.IsDeleted)
            .ProjectTo<GoverningAgencySimpleDto>(_mapper.ConfigurationProvider);

        var list = await query.ToListAsync();

        return await Result<List<GoverningAgencySimpleDto>>.SuccessAsync(list);
    }
}
