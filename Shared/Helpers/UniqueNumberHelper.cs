namespace Shared.Helpers;

public class UniqueNumberHelper
{
    public static string GenerateUniqueNumberSequence(int count = 12)
    {
        string guidString = Guid.NewGuid().ToString("N");
        string numericString = new string(guidString.Where(char.IsDigit).ToArray());

        if (numericString.Length < count)
        {
            while (numericString.Length < count)
            {
                guidString = Guid.NewGuid().ToString("N");
                numericString += new string(guidString.Where(char.IsDigit).ToArray());
            }
        }

        return numericString.Substring(0, count);
    }
}