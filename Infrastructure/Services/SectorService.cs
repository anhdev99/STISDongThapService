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

public class SectorService : ISectorService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public SectorService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<int>> Create(CreateSectorRequest model, CancellationToken cancellationToken)
    {
        var entity = new Sector()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
        };

        _context.Sectors.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Ngành tạo thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateSectorRequest model, CancellationToken cancellationToken)
    {
        var entity = _context.Sectors.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy ngành: {id}");
        }
        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;

        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật ngành thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _context.Sectors.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy ngành: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa ngành thành công");
    }

    public async Task<PaginatedResult<GetSectorWithPagingDto>> GetSectorsWithPaging(GetSectorsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _context.Sectors.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.OrderByDescending(x => x.Order)
            .ProjectTo<GetSectorWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetSectorDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Sectors.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetSectorDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetSectorDto>.SuccessAsync(entity);
    }
    
    public async Task<Result<List<SectorSimpleDto>>> GetAll()
    {
        var list = _context.Sectors.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ProjectTo<SectorSimpleDto>(_mapper.ConfigurationProvider)
            .ToList();
        return await Result<List<SectorSimpleDto>>.SuccessAsync(list);
    }
}