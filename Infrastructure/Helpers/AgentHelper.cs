namespace Infrastructure.Helpers;

public static class AgentHelper
{
    public static string GetDeviceType(this string userAgent)
    {
        bool isMobile = userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone");

        return isMobile ? "mobile" : "web";
    }
}