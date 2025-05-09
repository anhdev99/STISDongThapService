using Microsoft.AspNetCore.Http;

namespace Core.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var profileCode = context.Request.Headers["x-profile-code"].ToString();
        var userName = jwtUtils.ValidateJwtToken(token);
        if (!string.IsNullOrEmpty(userName))
        {
            // var user = await userService.GetUserByUserNameAsync(userName);
            // if (user != null)
            // {
            //     context.Items["UserName"] = userName;
            //     context.Items["User"] = user;
            //     if(!string.IsNullOrEmpty(profileCode))
            //     {
            //         var profile = await profileService.GetProfileByUserNameAndProfileCode(userName, profileCode);
            //         if (profile != null)
            //         {
            //             var roles = roleService.GetRoleByProfileCode(userName, profile.ProfileCode);
            //             context.Items["Profile"] = profile;
            //             context.Items["ProfileCode"] = profile.ProfileCode;
            //             context.Items["Roles"] = roles;
            //         } 
            //     }
            // }
        }

        await _next(context);
    }
}