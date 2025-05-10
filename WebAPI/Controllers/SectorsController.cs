using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class SectorsController(ISectorService SectorService, ILogger<SectorsController> logger) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreateSectorRequest request, CancellationToken cancellationToken)
    {
        return await SectorService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdateSectorRequest request,
        CancellationToken cancellationToken)
    {
        return await SectorService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await SectorService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetSectorDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await SectorService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("GetDevicesWithPaging")]
    public async Task<ActionResult<PaginatedResult<GetSectorWithPagingDto>>> GetSectorWithPaging(
        [FromQuery] GetSectorsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await SectorService.GetSectorsWithPaging(query, cancellationToken);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<Result<List<SectorSimpleDto>>>> GetAll()
    {
        return await SectorService.GetAll();
    }
}