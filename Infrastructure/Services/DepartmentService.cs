using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Extensions;

namespace Infrastructure.Services;

public class DepartmentService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IDepartmentService
{
    private readonly IMapper _mapper = mapper;
    
    private List<string> FlattenDepartments(List<DepartmentTreeDto> departments, int level = 0)
    {
        var result = new List<string>();
        string prefix = new string('-', level * 2) + "| ";

        foreach (var dept in departments)
        {
            result.Add(prefix + dept.Name);
            result.AddRange(FlattenDepartments(dept.Children, level + 1));
        }

        return result;
    }

    private List<DepartmentTreeDto> BuildTreeWithFormattedName(List<DepartmentTreeDto> flatList)
    {
        var lookup = flatList.ToDictionary(d => d.Id);
        var rootList = new List<DepartmentTreeDto>();

        foreach (var dept in flatList)
        {
            if (dept.ParentId == null)
            {
                rootList.Add(dept);
            }
            else if (lookup.TryGetValue(dept.ParentId.Value, out var parent))
            {
                parent.Children.Add(dept);
            }
        }

        FlattenDepartments(rootList, 0);

        return rootList;
    }

    
    
    private async Task<List<Department>> GetDepartmentsWithCode(string departmentCode,
        CancellationToken cancellationToken = default)
    {
        var data = new List<Department>();

        if (!string.IsNullOrEmpty(departmentCode))
        {
            var departmentParent = await _unitOfWork.Repository<Department>().Entities
                .FirstOrDefaultAsync(x => x.Code == departmentCode && x.IsDeleted != true, cancellationToken);
            if (departmentParent == null)
                throw new Exception("Không tìm thấy phòng ban");

            data = await _unitOfWork.Repository<Department>().Entities
                .Where(x => x.ParentId == departmentParent.Id && x.IsDeleted == false).ToListAsync(cancellationToken);
        }
        else
        {
            data = await _unitOfWork.Repository<Department>().Entities.Where(x => x.IsDeleted == false)
                .ToListAsync(cancellationToken);
        }

        return data;
    }
    
    private List<DepartmentTreeDto> FlattenDepartmentTree(List<DepartmentTreeDto> tree)
    {
        var result = new List<DepartmentTreeDto>();

        void Traverse(DepartmentTreeDto node, int level)
        {
            string prefix = new string('-', level * 2) + "| ";
            var flatNode = new DepartmentTreeDto()
            {
                Id = node.Id,
                Code = node.Code,
                Name = prefix + node.Name.TrimStart('-', '|', ' '),
                ParentId = node.ParentId,
                Children = new List<DepartmentTreeDto>()
            };
            result.Add(flatNode);
            foreach (var child in node.Children)
            {
                Traverse(child, level + 1);
            }
        }

        foreach (var root in tree)
        {
            Traverse(root, 0);
        }

        return result;
    }

    public async Task<List<DepartmentTreeDto>> GetDepartments(string departmentCode,
        CancellationToken cancellationToken = default)
    {
        var data = await GetDepartmentsWithCode(departmentCode, cancellationToken);

        var flatList = data.Select(x => new DepartmentTreeDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            ParentId = x.ParentId,
            Children = new List<DepartmentTreeDto>()
        }).ToList();
        var tree = BuildTreeWithFormattedName(flatList);
        var departmentResult = FlattenDepartmentTree(tree);
        return departmentResult;
    }


    public async Task<Result<int>> Create(CreateDepartmentRequest model, CancellationToken cancellationToken)
    {
        var existingStatus = await _unitOfWork.Repository<Department>().Entities
            .AnyAsync(x => x.Code == model.Code && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã đơn vị đã tồn tại");
        }
        
        var entity = new Department()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
            ParentId = model.parentId
        };

        await _unitOfWork.Repository<Department>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Phòng ban đã được tạo");
    }
    
    public async Task<Result<int>> Update(int id, UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Department>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy phòng ban: {id}");
        }
        
        var existingStatus = await _unitOfWork.Repository<Department>().Entities
            .AnyAsync(x => x.Code == request.Code.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);

        if (existingStatus)
        {
            throw new Exception("Mã vai trò đã tồn tại");
        }
        
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Order = request.Order;
        entity.ParentId = request.parentId;

        await _unitOfWork.Repository<Department>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        
        _logger.LogInformation($"Phòng ban {request.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật Phòng ban thành công");    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Department>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy phòng ban: {id}");
        }

        entity.IsDeleted = true;

        await _unitOfWork.Repository<Department>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        _logger.LogInformation($"Phòng ban {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa Phòng ban thành công");
        
    }

    public async Task<PaginatedResult<GetDepartmentWithPagingDto>> GetDepartmentsWithPaging(
        GetDepartmentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Department>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(request.Keywords))
            query = query.Where(x => x.Name.ToLower().Trim().Contains(request.Keywords.ToLower().Trim()));

        return await query.OrderByDescending(x => x.ParentId)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetDepartmentWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetDepartmentDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Department>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<GetDepartmentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Menu không tồn tại");
        }

        return await Result<GetDepartmentDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<GetDepartmentDto>>> GetAllDepartment(CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Department>().Entities
            .Where(x => x.IsDeleted != true)
            .ProjectTo<GetDepartmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return await Result<List<GetDepartmentDto>>.SuccessAsync(entity);

    }
    
    public async Task<Result<List<DepartmentResponse>>> GetFullDepartmentTree(CancellationToken cancellationToken = default)
    {
        var allDepartments = await _unitOfWork.Repository<Department>().Entities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        var lookup = allDepartments.ToLookup(d => d.ParentId);

        async Task<List<DepartmentResponse>> BuildTree(int? parentId)
        {
            var children = lookup[parentId]
                .OrderBy(d => d.Order)
                .Select(async dept => new DepartmentResponse
                {
                    Id = dept.Id,
                    Code = dept.Code,
                    Name = dept.Name,
                    Order = dept.Order,
                    ParentId = dept.ParentId,
                    Children = await BuildTree(dept.Id)
                });

            var results = await Task.WhenAll(children);
            return results.ToList();
        }

        var departmentTree = await BuildTree(null);
        return Result<List<DepartmentResponse>>.Success(departmentTree);
    }
}