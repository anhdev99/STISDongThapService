using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class RanksController(IRankService rankService, ILogger<RanksController> logger) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreateRankRequest request, CancellationToken cancellationToken)
    {
        return await rankService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdateRankRequest request,
        CancellationToken cancellationToken)
    {
        return await rankService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await rankService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetRankDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await rankService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetRankWithPagingDto>>> GetRankWithPaging(
        [FromQuery] GetRanksWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await rankService.GetRanksWithPaging(query, cancellationToken);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<Result<List<RankSimpleDto>>>> GetAll()
    {
        return await rankService.GetAll();
    }
}