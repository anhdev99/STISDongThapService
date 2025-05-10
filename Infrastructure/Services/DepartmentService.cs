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
    private readonly ApplicationDbContext _context = dbContext;
    private readonly IMapper _mapper = mapper;
    
    public async Task<DepartmentResponse> GetDepartmentByCode(string departmentCode,
        CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Department>().Entities
            .Where(x => x.Code == departmentCode && x.IsDeleted != true)
            .ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        return entity;
    }
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

        // Add Children
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
                throw new Exception("Department not found");

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
        var entity = new Department()
        {
            Code = model.Code,
            Name = model.Name,
            Order = model.Order,
            ParentId = model.parentId
        };

        _context.Departments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Department created");
    }
    
    public async Task<Result<int>> Update(int id, UpdateDepartmentRequest model, CancellationToken cancellationToken)
    {
        var entity = _context.Departments.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Department not found: {id}");
        }
        entity.Code = model.Code;
        entity.Name = model.Name;
        entity.Order = model.Order;
        entity.ParentId = model.parentId;

        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Department updated");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = _context.Departments.FirstOrDefault(x => x.Id == id && x.IsDeleted != true);
        if (entity == null)
        {
            throw new Exception($"Department not found: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Department deleted");
    }

    public async Task<PaginatedResult<GetDepartmentWithPagingDto>> GetDepartmentsWithPaging(GetDepartmentsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var filteredQuery = _context.Departments.AsQueryable();

        filteredQuery = filteredQuery.Where(x => !x.IsDeleted).OrderBy(x => x.Order);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords));
        }

        return await filteredQuery.OrderByDescending(x => x.Order)
            .ProjectTo<GetDepartmentWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetDepartmentDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Departments.Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetDepartmentDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        return await Result<GetDepartmentDto>.SuccessAsync(entity);
    }
}