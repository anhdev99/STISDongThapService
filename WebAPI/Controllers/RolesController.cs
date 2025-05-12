using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class RolesController(ILogger<RolesController> logger, IRoleService roleService) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    // api them moi quyen
    public async Task<ActionResult<Result<int>>> Create([FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await roleService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update([FromRoute] int id, [FromBody] UpdateRoleRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != request.id)
        {
            throw new Exception("Mã id không trùng với id trong body");
        }

        return await roleService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await roleService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetRoleDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await roleService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetRoleWithPaginationDto>>> GetRolesWithPagination(
        [FromQuery] GetRolesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await roleService.GetRolesWithPagination(query, cancellationToken);
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<ActionResult<Result<List<GetRoleDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return await roleService.GetAll(cancellationToken);
    }

    [HttpPost]
    [Route("assign-role-menus/{id}")]
    public async Task<ActionResult<Result<int>>> AssignRoleMenus([FromRoute] int id, [FromBody] RoleMenuRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != request.RoleId)
        {
            throw new Exception("Id trong URL không khớp với Id trong body");
        }

        return await roleService.AssignRoleMenus(request, cancellationToken);
    }

    [HttpPost]
    [Route("assign-role-permissions/{id}")]
    public async Task<ActionResult<Result<int>>> AssignRolePermissions([FromRoute] int id,
        [FromBody] RolePermissionRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != request.RoleId)
        {
            throw new Exception("Id trong URL không khớp với Id trong body");
        }

        return await roleService.AssignRolePermissions(request, cancellationToken);
    }

    [HttpGet]
    [Route("get-config-menu/{id}")]
    public async Task<ActionResult<Result<List<int>>>> GetConfigMenu([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        return await roleService.GetConfigMenuByRoleId(id, cancellationToken);
    }
    
    [HttpPost]
    [Route("config-user")]
    public async Task<ActionResult<Result<int>>> GetConfigUser([FromBody] ConfigUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        return await roleService.ConfigUserRole(request, cancellationToken);
    }
    [HttpGet]
    [Route("get-permissions")]
    public async Task<ActionResult<Result<List<RolePermissionDto>>>> GetPermissions(CancellationToken cancellationToken)
    {
        return await roleService.GetPermissions(cancellationToken);
    }
    [HttpGet]
    [Route("get-permissions-by-role/{id}")]
    public async Task<ActionResult<Result<List<string>>>> GetConfigPermissionByRoleId([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        return await roleService.GetConfigPermissionByRoleId(id, cancellationToken);
    }
    [HttpPost]
    [Route("config-permission-role/{id}")]
    public async Task<ActionResult<Result<int>>> ConfigPermissionRole([FromBody] ConfigPermissionRoleRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request.Id != request.Id)
        {
            throw new Exception("Id trong URL không khớp với Id trong body");
        }

        return await roleService.ConfigPermissionRole(request, cancellationToken);
    }
}