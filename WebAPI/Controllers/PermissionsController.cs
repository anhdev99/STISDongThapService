using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class PermissionsController(ILogger<PermissionsController> logger, IPermissionService permissionService)
    : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create([FromBody] CreatePermissionRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await permissionService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update([FromRoute] int id, [FromBody] UpdatePermissionRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await permissionService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await permissionService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetPermissionDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await permissionService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetPermissionWithPaginationDto>>> GetPermissionsWithPagination(
        [FromQuery] GetPermissionsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await permissionService.GetPermissionsWithPagination(query, cancellationToken);
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<ActionResult<Result<List<GetPermissionDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return await permissionService.GetAll(cancellationToken);
    }
}