using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core.Interfaces.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Core.Authorization;

public interface IJwtUtils
{
    string? ValidateJwtToken(string token);
}

public class JwtUtils(IJwtSettings jwtSettings) : IJwtUtils
{
    public string? ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userName = jwtToken.Claims.First(x => x.Type == "username").Value;

            return userName;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}