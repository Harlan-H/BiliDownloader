using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Text.Json;

namespace BiliDownloader.Core.Extractors
{
    internal class VideoJsonDataExtractor
    {
        private readonly JsonElement jsonElement;

        public VideoJsonDataExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public string? TryGetVideoTitle() => Memory.Cache(this, () =>
                jsonElement
                .GetPropertyOrNull("part")?
                .GetStringOrNull()
        );

        public int? TryGetPage() => Memory.Cache(this, () =>
            jsonElement
                .GetPropertyOrNull("page")?
                .GetInt32OrNull()
        );

        public int? TryGetCid() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("cid")?
            .GetInt32OrNull()
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
             .GetPropertyOrNull("first_frame")?
             .GetStringOrNull()
        );
    }
}
