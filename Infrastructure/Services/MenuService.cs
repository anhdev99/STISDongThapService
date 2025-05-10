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

public class MenuService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<MenuService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger,
        dbContext,
        unitOfWork, mapper), IMenuService
{
    public async Task<Result<int>> Create(CreateMenuRequest request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        var existingMenu = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.Name.Equals(name) && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingMenu != null)
        {
            throw new Exception("Menu đã tồn tại");
        }

        bool isParent = request.ParentId.HasValue && request.ParentId != 0;
        if (isParent)
        {
            var parentMenu = await _unitOfWork.Repository<Menu>().Entities
                .Where(x => x.Id == request.ParentId && x.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);
            if (parentMenu == null)
            {
                throw new Exception("Không tìm thấy menu cha");
            }
        }

        var entity = new Menu
        {
            Name = name,
            Url = request.Url,
            Description = request.Description,
            Icon = request.Icon,
            IsBlank = request.IsBlank,
            Order = request.Order,
            ParentId = isParent ? request.ParentId.Value : null,
        };
        await _unitOfWork.Repository<Menu>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);
        _logger.LogInformation($"Menu {request.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo menu thành công");
    }

    public async Task<Result<int>> Update(int id, UpdateMenuRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Menu không tồn tại");
        }

        var name = request.Name.Trim();
        var existingMenu = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.Name.Equals(name) && x.Id != id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingMenu != null)
        {
            throw new Exception("Menu đã tồn tại");
        }

        if (request.ParentId.HasValue && request.ParentId != 0)
        {
            var parentMenu = await _unitOfWork.Repository<Menu>().Entities
                .Where(x => x.Id == request.ParentId && x.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);
            if (parentMenu == null)
            {
                throw new Exception("Không tìm thấy menu cha");
            }
            else
            {
                entity.ParentId = parentMenu.Id;
            }
        }

        entity.Name = name;
        entity.Url = request.Url;
        entity.Description = request.Description;
        entity.Icon = request.Icon;
        entity.IsBlank = request.IsBlank;
        entity.Order = request.Order;

        await _unitOfWork.Repository<Menu>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Menu {request.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật menu thành công");
    }

    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception("Menu không tồn tại");
        }

        entity.IsDeleted = true;
        await _unitOfWork.Repository<Menu>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Menu {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa menu thành công");
    }

    public async Task<PaginatedResult<GetMenuWithPaginationDto>> GetMenusWithPagination(
        GetMenusWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Menu>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(request.Keywords))
            query = query.Where(x => x.Name.ToLower().Trim().Contains(request.Keywords.ToLower().Trim()));

        if (request.ParentId != null)
            query = query.Where(x => x.ParentId == request.ParentId);

        if (request.RoleIds != null)
            query = query.Where(x => x.RoleMenus != null && x.RoleMenus.Any(p => request.RoleIds.Contains(p.Id)));

        return await query.OrderByDescending(x => x.ParentId)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetMenuWithPaginationDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }

    public async Task<Result<GetMenuDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<GetMenuDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Menu không tồn tại");
        }

        return await Result<GetMenuDto>.SuccessAsync(entity);
    }

    public async Task<Result<List<GetMenuDto>>> GetAll(CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.IsDeleted == false)
            .ProjectTo<GetMenuDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (data.Count <= 0)
        {
            throw new Exception("Không tìm thấy menu nào");
        }

        return await Result<List<GetMenuDto>>.SuccessAsync(data);
    }

    public async Task<Result<List<GetMenuTreeViewDto>>> GetTreeView(CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.IsDeleted == false && x.ParentId == null)
            .ProjectTo<GetMenuTreeViewDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (data.Count <= 0)
        {
            throw new Exception("Không tìm thấy menu nào");
        }

        foreach (var item in data)
        {
            item.Children = await GetChildren(item.Id, cancellationToken);
        }


        return await Result<List<GetMenuTreeViewDto>>.SuccessAsync(data);
    }

    private async Task<List<GetMenuTreeViewDto>> GetChildren(int? parentId, CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.Repository<Menu>().Entities
            .Where(x => x.IsDeleted == false && x.ParentId == parentId)
            .ProjectTo<GetMenuTreeViewDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        foreach (var item in data)
        {
            item.Children = await GetChildren(item.Id, cancellationToken);
        }

        return data;
    }


    public async Task<Result<List<GetMenuTreeViewDto>>> GetMenusByUserRoles(CancellationToken cancellationToken)
    {
        var roleIds = await _unitOfWork.Repository<Role>().Entities
            .Where(x => Roles.Contains(x.Code) && x.IsDeleted != true).Select(x => x.Id).ToListAsync(cancellationToken);

        var roleMenus = await _unitOfWork.Repository<RoleMenu>().Entities
            .Where(x => roleIds.Contains(x.RoleId) && x.IsDeleted != true).ToListAsync(cancellationToken);

        var menus = _unitOfWork.Repository<Menu>().Entities.Where(x => x.IsDeleted != true)
            .ProjectTo<GetMenuTreeViewDto>(_mapper.ConfigurationProvider).ToList();

        var menuIds = roleMenus.Select(x => x.MenuId).Distinct().ToList();

        var menusAll = menus.Where(x => menuIds.Contains(x.Id)).ToList();
        var buildTree = FlattenMenu(menusAll);

        return await Result<List<GetMenuTreeViewDto>>.SuccessAsync(buildTree);
    }

    public static List<GetMenuTreeViewDto> FlattenMenu(List<GetMenuTreeViewDto> allMenus)
    {
        var childMap = allMenus
            .Where(m => m.ParentId != null)
            .GroupBy(m => m.ParentId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        HashSet<int> included = new();
        List<GetMenuTreeViewDto> result = new();

        foreach (var root in allMenus.Where(m => m.ParentId == null))
        {
            var rootCopy = new GetMenuTreeViewDto
            {
                Id = root.Id,
                Name = root.Name,
                Icon = root.Icon,
                Url = root.Url,
                IsBlank = root.IsBlank,
                ParentId = null,
                Children = null
            };
            result.Add(rootCopy);
            included.Add(root.Id);

            if (childMap.ContainsKey(root.Id))
            {
                foreach (var child in childMap[root.Id])
                {
                    included.Add(child.Id);
                    var newChild = DeepCloneWithChildren(child, childMap, included);
                    result.Add(newChild);
                }
            }
        }

        return result;
    }

    private static GetMenuTreeViewDto DeepCloneWithChildren(GetMenuTreeViewDto node,
        Dictionary<int, List<GetMenuTreeViewDto>> childMap, HashSet<int> included)
    {
        var copy = new GetMenuTreeViewDto
        {
            Id = node.Id,
            Name = node.Name,
            Icon = node.Icon,
            Url = node.Url,
            IsBlank = node.IsBlank,
            ParentId = node.ParentId,
            Children = null
        };

        if (childMap.ContainsKey(node.Id))
        {
            copy.Children = new List<GetMenuTreeViewDto>();
            foreach (var child in childMap[node.Id])
            {
                if (!included.Contains(child.Id))
                {
                    included.Add(child.Id);
                    copy.Children.Add(DeepCloneWithChildren(child, childMap, included));
                }
            }
        }

        return copy;
    }
}
