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

public class HostOrganizationService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IHostOrganizationService
{
    public async Task<Result<int>> Create(CreateHostOrganizationRequest model, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.Repository<HostOrganization>().Entities
            .AnyAsync(x => x.Name == model.Name && !x.IsDeleted, cancellationToken);

        if (exists)
            throw new Exception("Tên tổ chức chủ trì đã tồn tại");

        var entity = new HostOrganization
        {
            Name = model.Name,
            TaxCode = model.TaxCode,
            Representative = model.Representative,
            Position = model.Position,
            Phone = model.Phone,
            Email = model.Email,
            Address = model.Address,
            Website = model.Website,
            Order = model.Order
        };

        await _unitOfWork.Repository<HostOrganization>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"HostOrganization {model.Name} created");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo tổ chức chủ trì thành công");
    }

    public async Task<Result<int>> Update(int id, UpdateHostOrganizationRequest model, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<HostOrganization>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
            throw new Exception($"Không tìm thấy tổ chức chủ trì: {id}");

        var exists = await _unitOfWork.Repository<HostOrganization>().Entities
            .AnyAsync(x => x.Name == model.Name.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);
        if (exists)
            throw new Exception("Tên tổ chức chủ trì đã tồn tại");

        entity.Name = model.Name;
        entity.TaxCode = model.TaxCode;
        entity.Representative = model.Representative;
        entity.Position = model.Position;
        entity.Phone = model.Phone;
        entity.Email = model.Email;
        entity.Address = model.Address;
        entity.Website = model.Website;
        entity.Order = model.Order;

        await _unitOfWork.Repository<HostOrganization>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"HostOrganization {model.Name} updated");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật tổ chức chủ trì thành công");
    }

    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _unitOfWork.Repository<HostOrganization>().Entities.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
            throw new Exception($"Tổ chức chủ trì không tìm thấy: {id}");

        entity.IsDeleted = true;
        await _unitOfWork.Repository<HostOrganization>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"HostOrganization {entity.Name} deleted");
        return await Result<int>.SuccessAsync(id, "Xóa tổ chức chủ trì thành công");
    }

    public async Task<PaginatedResult<GetHostOrganizationWithPagingDto>> GetHostOrganizationsWithPaging(GetHostOrganizationsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<HostOrganization>().Entities.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
            filteredQuery = filteredQuery.Where(x => x.Name.ToLower().Trim().Contains(query.Keywords.ToLower().Trim()));

        return await filteredQuery.OrderByDescending(x => x.Name)
            .ProjectTo<GetHostOrganizationWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }

    public async Task<Result<GetHostOrganizationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<HostOrganization>().Entities.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetHostOrganizationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new Exception("Tổ chức chủ trì không tồn tại");

        return await Result<GetHostOrganizationDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<HostOrganizationSimpleDto>>> GetAll()
    {
        var query = _unitOfWork.Repository<HostOrganization>()
            .Entities
            .Where(x => !x.IsDeleted)
            .ProjectTo<HostOrganizationSimpleDto>(_mapper.ConfigurationProvider);

        var list = await query.ToListAsync();

        return await Result<List<HostOrganizationSimpleDto>>.SuccessAsync(list);
    }
}
