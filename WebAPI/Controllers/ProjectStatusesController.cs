using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class ProjectStatusesController(IStatusService Statuseservice, ILogger<ProjectStatusesController> logger) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreateStatusRequest request, CancellationToken cancellationToken)
    {
        return await Statuseservice.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        return await Statuseservice.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await Statuseservice.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetStatusDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await Statuseservice.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("GetDevicesWithPaging")]
    public async Task<ActionResult<PaginatedResult<GetStatusesWithPagingDto>>> GetRankWithPaging(
        [FromQuery] GetStatusesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await Statuseservice.GetStatusesWithPaging(query, cancellationToken);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<Result<List<StatusSimpleDto>>>> GetAll()
    {
        return await Statuseservice.GetAll();
    }
}