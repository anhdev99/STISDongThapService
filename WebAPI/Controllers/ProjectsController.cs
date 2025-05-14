using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class ProjectsController(IProjectService ProjectService, ILogger<ProjectsController> logger) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        return await ProjectService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        return await ProjectService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await ProjectService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetProjectDetailDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await ProjectService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetProjectWithPagingDto>>> GetProjectWithPaging(
        [FromQuery] GetProjectsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await ProjectService.GetProjectsWithPaging(query, cancellationToken);
    }
}