using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Extractors
{
    internal partial class VideoJsonExtractor
    {
        private readonly JsonElement jsonElement;

        public VideoJsonExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public IReadOnlyList<VideoJsonDataExtractor> TryGetVideoData() => Memory.Cache(this, () => 
            jsonElement
            .GetPropertyOrNull("data")?
            .EnumerateArrayOrNull()?
            .Select(i => new VideoJsonDataExtractor(i))
            .ToArray() ??
            Array.Empty<VideoJsonDataExtractor>()
        );

        public bool IsVideoAvailable() => Memory.Cache(this, () =>
            jsonElement
            .GetProperty("code")
            .GetInt32() == 0
        );

    }

    internal partial class VideoJsonExtractor
    {
        public static VideoJsonExtractor TypCreate(string raw) => new(Json.Parse(raw));
    }
}
