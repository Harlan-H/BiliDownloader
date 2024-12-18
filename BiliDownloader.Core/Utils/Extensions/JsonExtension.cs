using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    public static class JsonExtension
    {
        public static JsonElement? GetPropertyOrNull(this JsonElement element,string property)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return null;

            if (element.TryGetProperty(property, out var result) && result.ValueKind != JsonValueKind.Null && result.ValueKind != JsonValueKind.Undefined)
                return result;

            return null;
        }

        public static string? GetStringOrNull(this JsonElement element) =>
            element.ValueKind == JsonValueKind.String ? element.GetString() : null;

        public static int? GetInt32OrNull(this JsonElement element) =>
            element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var result) ? result : null;

        public static long? GetInt64OrNull(this JsonElement element) =>
            element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var result) ? result : null;

        public static JsonElement.ArrayEnumerator? EnumerateArrayOrNull(this JsonElement element) =>
            element.ValueKind == JsonValueKind.Array ? element.EnumerateArray() : null;

        public static double? GetDoubleOrNull(this JsonElement element) =>
            element.ValueKind == JsonValueKind.Number && element.TryGetDouble(out var result) ? result : null;
    }
}
