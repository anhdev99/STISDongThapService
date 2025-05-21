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

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null && userId.HasValue)
        {
            var user = await userService.GetById(userId.Value);
            if (user != null)
            {
                context.Items["UserName"] = user.UserName;
                context.Items["User"] = user;
                // if(!string.IsNullOrEmpty(profileCode))
                // {
                //     var profile = await profileService.GetProfileByUserNameAndProfileCode(userName, profileCode);
                //     if (profile != null)
                //     {
                //         var roles = roleService.GetRoleByProfileCode(userName, profile.ProfileCode);
                //         context.Items["Profile"] = profile;
                //         context.Items["ProfileCode"] = profile.ProfileCode;
                //         context.Items["Roles"] = roles;
                //     } 
                // }
            }
        }

        await _next(context);
    }
}