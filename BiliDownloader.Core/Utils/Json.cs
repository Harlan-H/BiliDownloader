using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils
{
    public static class Json
    {
        public static JsonElement Parse(string source)
        {
            using var jsonelement = JsonDocument.Parse(source);
            return jsonelement.RootElement.Clone();
        }

        public static JsonElement? TryParse(string source)
        {
            try
            {
                return Parse(source);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
