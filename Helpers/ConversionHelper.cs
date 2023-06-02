using System.Globalization;

namespace backend.Helpers;

public static class ConversionHelper
{
    public static DateTime ConvertToDateTime(string date, string time)
    {
        var datetimeString = $"{date} {time}";
        return DateTime.ParseExact(datetimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
    }
}