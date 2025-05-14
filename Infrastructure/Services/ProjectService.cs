using System.Text.Json;
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

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public ProjectService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<int>> Create(CreateProjectRequest model, CancellationToken cancellationToken)
    {
        var entity = new Project
        {
            Name = model.Name,
            Content = model.Content,
            Target = model.Target,
            SectorId = model.SectorId,
            StatusId = model.StatusId,
            ManagementLevelId = model.ManagementLevelId,
            TaskTypeId = model.TaskTypeId,
            ProjectLeaderId = model.ProjectLeaderId,
            HostOrganizationId = model.HostOrganizationId,
            GoverningAgencyId = model.GoverningAgencyId,
            RankId = model.RankId
        };
        
        string json = JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);
        _context.Projects.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Dự án tạo thành công");
    }
    
    public async Task<Result<int>> Update(int id, UpdateProjectRequest model, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != true, cancellationToken);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy dự án: {id}");
        }

        entity.Name = model.Name;
        entity.Content = model.Content;
        entity.Target = model.Target;
        entity.SectorId = model.SectorId;
        entity.StatusId = model.StatusId;
        entity.ManagementLevelId = model.ManagementLevelId;
        entity.TaskTypeId = model.TaskTypeId;
        entity.ProjectLeaderId = model.ProjectLeaderId;
        entity.HostOrganizationId = model.HostOrganizationId;
        entity.GoverningAgencyId = model.GoverningAgencyId;
        entity.RankId = model.RankId;

        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật dự án thành công");
    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != true, cancellationToken);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy dự án: {id}");
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(entity.Id, "Xóa dự án thành công");
    }

    public async Task<PaginatedResult<GetProjectWithPagingDto>> GetProjectsWithPaging(
        GetProjectsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _context.Projects.AsQueryable().Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(query.Keywords))
        {
            filteredQuery = filteredQuery.Where(x => x.Name.Contains(query.Keywords) || x.Content.Contains(query.Keywords));
        }

        return await filteredQuery
            .ProjectTo<GetProjectWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetProjectDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects
            .Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetProjectDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return await Result<GetProjectDetailDto>.SuccessAsync(entity);
    }
}