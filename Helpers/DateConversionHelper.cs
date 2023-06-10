using System.Globalization;

namespace InvesTime.BackEnd.Helpers;

public static class DateConversionHelper
{
    public static DateTime ConvertToDateTime(string date, string time)
    {
        var datetimeString = $"{date} {time}";
        return DateTime.ParseExact(datetimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
    }
}