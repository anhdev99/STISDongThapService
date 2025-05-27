using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;


[Authorize]
public class PositionsController(IPositionService positionService, ILogger<SectorsController> logger) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreatePositionRequest request, CancellationToken cancellationToken)
    {
        return await positionService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdatePositionRequest request,
        CancellationToken cancellationToken)
    {
        return await positionService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await positionService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetPositionDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await positionService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetPositionsWithPagingDto>>> GetSectorWithPaging(
        [FromQuery] GetPositionsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await positionService.GetPositionsWithPaging(query, cancellationToken);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<Result<List<PositionSimpleDto>>>> GetAll()
    {
        return await positionService.GetAll();
    }
}