using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class TaskTypesController(ITaskTypeService TaskTypeService, ILogger<TaskTypesController> logger) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create(CreateTaskTypeRequest request, CancellationToken cancellationToken)
    {
        return await TaskTypeService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update(int id, UpdateTaskTypeRequest request,
        CancellationToken cancellationToken)
    {
        return await TaskTypeService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete(int id, CancellationToken cancellationToken)
    {
        return await TaskTypeService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetTaskTypeDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await TaskTypeService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("GetDevicesWithPaging")]
    public async Task<ActionResult<PaginatedResult<GetTaskTypeWithPagingDto>>> GetTaskTypeWithPaging(
        [FromQuery] GetTaskTypesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await TaskTypeService.GetTaskTypesWithPaging(query, cancellationToken);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<Result<List<TaskTypeSimpleDto>>>> GetAll()
    {
        return await TaskTypeService.GetAll();
    }
}