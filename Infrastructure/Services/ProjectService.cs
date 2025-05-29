using System.Text.Json;
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

public class ProjectService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper), IProjectService
{
    
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
        
        await _unitOfWork.Repository<Project>().AddAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Dự án {model.Name} đã được tạo");
        return await Result<int>.SuccessAsync(entity.Id, "Tạo dự án thành công");
        
    }
    
    public async Task<Result<int>> Update(int id, UpdateProjectRequest model, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Project>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
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

        await _unitOfWork.Repository<Project>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Dự án {model.Name} đã được cập nhật");
        return await Result<int>.SuccessAsync(entity.Id, "Cập nhật dự án thành công");    }
    
    public async Task<Result<int>> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Project>().Entities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            throw new Exception($"Không tìm thấy dự án: {id}");
        }

        entity.IsDeleted = true;

        await _unitOfWork.Repository<Project>().UpdateAsync(entity);
        await _unitOfWork.Save(cancellationToken);

        _logger.LogInformation($"Dự án {entity.Name} đã được xóa");
        return await Result<int>.SuccessAsync(id, "Xóa Dự án thành công");
    }

    public async Task<PaginatedResult<GetProjectWithPagingDto>> GetProjectsWithPaging(
        GetProjectsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var filteredQuery = _unitOfWork.Repository<Project>().Entities.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(query.Keywords))
            filteredQuery = filteredQuery.Where(x => x.Name.ToLower().Trim().Contains(query.Keywords.ToLower().Trim()));

        return await filteredQuery.OrderByDescending(x => x.Name)
            .ThenByDescending(x => x.UpdatedDate)
            .ProjectTo<GetProjectWithPagingDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
    
    public async Task<Result<GetProjectDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Project>().Entities
            .Where(x => x.Id == id && x.IsDeleted != true)
            .ProjectTo<GetProjectDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new Exception("Dự án không tồn tại");
        }

        return await Result<GetProjectDetailDto>.SuccessAsync(entity);    
    }
}