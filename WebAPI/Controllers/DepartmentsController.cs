using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

public class DepartmentsController(IMediator mediator, ILogger<DepartmentsController> logger, IDepartmentService departmentService)
    : ApiControllerBase( logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        return await departmentService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        return await departmentService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await departmentService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetDepartmentDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await departmentService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetDepartmentWithPagingDto>>> GetDepartmentWithPaging(
        [FromQuery] GetDepartmentsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await departmentService.GetDepartmentsWithPaging(query, cancellationToken);
    }
    
    [HttpGet]
    [Route("get-tree")]
    public async Task<Result<List<DepartmentTreeDto>>> GetDepartmentTree(CancellationToken cancellationToken)
    {
        return await Result<List<DepartmentTreeDto>>.SuccessAsync(await departmentService.GetDepartments("",cancellationToken));
    }
    
    [HttpGet]
    [Route("get-all")]
    public async Task<Result<List<GetDepartmentDto>>> GetDepartmentAll(CancellationToken cancellationToken)
    {
        return await departmentService.GetAllDepartment( cancellationToken);
    }
    
    [HttpGet("get-children")]
    public async Task<IActionResult> GetChildrenDepartments( CancellationToken cancellationToken)
    {
        var result = await departmentService.GetFullDepartmentTree( cancellationToken);
        return Ok(result);
    }
}