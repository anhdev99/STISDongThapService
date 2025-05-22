using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils, IUserService userService, IRoleService roleService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null && userId.HasValue)
        {
            var user = await userService.GetById(userId.Value);
            if (user != null)
            {
                context.Items["UserName"] = user.UserName;
                context.Items["UserId"] = user.Id;
                context.Items["User"] = user;
                if (user.Id > 0)
                {
                    var roles = roleService.GetRoleByProfileCode(user.Id);
                    context.Items["Roles"] = roles; 
                }
            }
        }

        await _next(context);
    }
}