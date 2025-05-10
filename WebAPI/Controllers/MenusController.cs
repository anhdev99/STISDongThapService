using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class MenusController(ILogger<MenusController> logger, IMenuService menuService) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create([FromBody] CreateMenuRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await menuService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update([FromRoute] int id, [FromBody] UpdateMenuRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await menuService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await menuService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetMenuDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await menuService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetMenuWithPaginationDto>>> GetMenusWithPagination(
        [FromQuery] GetMenusWithPaginationQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await menuService.GetMenusWithPagination(query, cancellationToken);
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<ActionResult<Result<List<GetMenuDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return await menuService.GetAll(cancellationToken);
    }

    [HttpGet]
    [Route("get-tree-view")]
    public async Task<ActionResult<Result<List<GetMenuTreeViewDto>>>> GetTreeView(CancellationToken cancellationToken)
    {
        return await menuService.GetTreeView(cancellationToken);
    }

    [HttpGet]
    [Route("GetMenusByUserRoles")]
    public async Task<ActionResult<Result<List<GetMenuTreeViewDto>>>> GetMenusByUserRoles(
        CancellationToken cancellationToken)
    {
        return await menuService.GetMenusByUserRoles(cancellationToken);
    }
}