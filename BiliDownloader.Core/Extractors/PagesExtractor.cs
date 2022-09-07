using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Text.Json;

namespace BiliDownloader.Core.Extractors
{
    internal class PagesExtractor
    {
        private readonly JsonElement jsonElement;

        public PagesExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public int? TryGetCid() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("cid")?
            .GetInt32OrNull()
        );

        public int? TryGetIndex() => Memory.Cache(this, () =>
             jsonElement
            .GetPropertyOrNull("page")?
            .GetInt32OrNull()
        );

        public string? TryGetTitle() => Memory.Cache(this, () =>
             jsonElement
            .GetPropertyOrNull("part")?
            .GetStringOrNull()
        );

        public TimeSpan? TryGetDuration() => Memory.Cache(this, () =>
             jsonElement
            .GetPropertyOrNull("duration")?
            .GetInt32OrNull()?
            .TryConvertDoubleOrNull()?
            .Pipe(TimeSpan.FromSeconds)
        );
    }
}
