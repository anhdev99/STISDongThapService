using Core.Authorization;
using Core.DTOs.Requests;
using Core.DTOs.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

[Authorize]
public class UsersController(ILogger<UsersController> logger, IUserService UserService)
    : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Result<int>>> Create([FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await UserService.Create(request, cancellationToken);
    }

    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<Result<int>>> Update([FromRoute] int id, [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await UserService.Update(id, request, cancellationToken);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<ActionResult<Result<int>>> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await UserService.Delete(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<ActionResult<Result<GetUserDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return await UserService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult<PaginatedResult<GetUserWithPaginationDto>>> GetUsersWithPagination(
        [FromQuery] GetUsersWithPaginationQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await UserService.GetUsersWithPagination(query, cancellationToken);
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<ActionResult<Result<List<GetUserDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return await UserService.GetAll(cancellationToken);
    }

    [HttpPost]
    [Route("verify")]
    public async Task<ActionResult<Result<bool>>> VerifyPassword([FromBody] VerifyRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await UserService.Verify(request.UserName, cancellationToken);
    }

    [HttpPost]
    [Route("change-password")]
    public async Task<ActionResult<Result<bool>>> ChangePassword([FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await UserService.ChangePassword(
            request.UserName, request.OldPassword, request.NewPassword, request.ConfirmPassword, cancellationToken);
    }

    // 🔁 Reset password
    [HttpPost]
    [Route("reset-password")]
    public async Task<ActionResult<Result<string>>> ResetPassword([FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return await UserService.ResetPassword(request.UserName, cancellationToken);
    }
}
