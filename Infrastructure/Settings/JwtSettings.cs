using Core.Interfaces.Settings;

namespace Infrastructure.Settings;

public class JwtSettings : IJwtSettings
{
    public string Secret { get; set; }
    public string TokenTimeLife { get; set; }
}