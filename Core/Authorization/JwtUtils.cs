using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.DTOs.Responses;
using Core.Entities;
using Core.Interfaces.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Core.Authorization;

public interface IJwtUtils
{
    (string, DateTime?) GenerateJwtToken(UserDto user);
    int? ValidateJwtToken(string token);
    RefreshToken GenerateRefreshToken(UserDto user, string ipAddress);
}

public class JwtUtils(IJwtSettings jwtSettings) : IJwtUtils
{
    public (string, DateTime?) GenerateJwtToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        TimeSpan tokenLifetime = TimeSpan.Parse(jwtSettings.TokenTimeLife);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("username", user.UserName) }),
            Expires = DateTime.UtcNow.AddHours(7).Add(tokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires);
    }

    public int? ValidateJwtToken(string token)
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
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return userId;
        }
        catch
        {
            return null;
        }
    }

    public RefreshToken GenerateRefreshToken(UserDto user, string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = GenerateHashRefreshToken(user),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            UserId = user.Id
        };

        return refreshToken;
    }

    private string GenerateHashRefreshToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        TimeSpan tokenLifetime = TimeSpan.Parse(jwtSettings.TokenTimeLife);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("username", user.UserName) }),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}