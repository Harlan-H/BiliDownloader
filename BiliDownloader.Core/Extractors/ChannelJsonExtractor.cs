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
    internal class ChannelJsonExtractor
    {
        private readonly JsonElement jsonElement;

        public ChannelJsonExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public bool IsVideoAvailable() => Memory.Cache(this, () =>
           jsonElement
           .GetProperty("code")
           .GetInt32() == 0
        );

        private JsonElement? TryGetVideoData() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("data")
        );

        public int TryGetTotal() => Memory.Cache(this, () =>
             TryGetVideoData()?
             .GetPropertyOrNull("page")?
             .GetPropertyOrNull("total")?
             .GetInt32OrNull() ?? 0
        );

        public IReadOnlyList<ChannelJsonDataExtractor> TryGetCannelData() => Memory.Cache(this, () =>
             TryGetVideoData()?
             .GetPropertyOrNull("archives")?
             .EnumerateArrayOrNull()?
             .Select(i => new ChannelJsonDataExtractor(i))
             .ToArray() ??
         Array.Empty<ChannelJsonDataExtractor>()
        );

    }
}
