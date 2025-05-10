namespace Shared.Helpers;

public static class NameHelper
{
    public static string GetFullName(string firstName, string? middleName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(middleName))
        {
            return $"{lastName} {firstName}";
        }
        return $"{lastName} {middleName} {firstName}";
    } 
    
    public static string GetLastName(this string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0] : string.Empty;
    }

    public static string GetFirstName(this string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[^1] : string.Empty;
    }

    public static string GetMiddleName(this string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length <= 2) return string.Empty;
        return string.Join(" ", parts.Skip(1).Take(parts.Length - 2));
    }

    public static string GetFirstAndLastName(this string fullName)
    {
        var first = fullName.GetFirstName();
        var last = fullName.GetLastName();
        return $"{last} {first}".Trim();
    }
}