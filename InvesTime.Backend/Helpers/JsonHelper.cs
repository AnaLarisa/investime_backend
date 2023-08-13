using System.Text.Json;

namespace InvesTime.BackEnd.Helpers;

public static class JsonHelper
{
    public static string GetStringOrDefault(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String)
        {
            return property.GetString();
        }

        return "";
    }
}
