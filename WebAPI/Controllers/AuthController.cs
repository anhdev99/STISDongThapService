using Core.Authorization;
using Core.DTOs;
using Core.DTOs.Requests;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;

public class AuthController(IIdentityService identityService, ILogger<AuthController> logger)
    : ApiControllerBase(logger)
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<Result<AuthenticationResponse>>> Create(LoginRequest command,
        CancellationToken cancellationToken)
    {
        var data = await identityService.LoginAsync(command, IpAddress(), GetUserAgent(), cancellationToken);
        return await Result<AuthenticationResponse>.SuccessAsync(data, "Đăng nhập thành công.");
    }

    private string IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    private string GetUserAgent()
    {
        return HttpContext.Request.Headers["User-Agent"].ToString();
    }
}
