namespace Core.Interfaces.Settings;

public interface IJwtSettings
{
    string Secret { get; set; }
    string TokenTimeLife { get; set; } 
}