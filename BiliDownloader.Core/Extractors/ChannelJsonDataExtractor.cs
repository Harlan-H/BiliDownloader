using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Text.Json;

namespace BiliDownloader.Core.Extractors
{
    internal class ChannelJsonDataExtractor
    {
        private readonly JsonElement jsonElement;

        public ChannelJsonDataExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public string? TryGetBvid() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("bvid")?
            .GetStringOrNull()
        );

        public string? TryGetVideoTitle() => Memory.Cache(this, () =>
               jsonElement
               .GetPropertyOrNull("title")?
               .GetStringOrNull()
       );


        public TimeSpan? TryGetDuration() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("duration")?
            .GetInt32OrNull()?
            .TryConvertDoubleOrNull()?
            .Pipe(TimeSpan.FromSeconds)
        );

        public string? TryGetThumbnail() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("pic")?
             .GetStringOrNull()
        );
    }
}
