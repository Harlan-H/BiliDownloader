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
    internal class EpisodesExtractor
    {
        private readonly JsonElement jsonElement;

        public EpisodesExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }


        public long? TryGetAid() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("aid")?
            .GetInt64OrNull()
        );

        public string? TryGetBvid() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("bvid")?
             .GetStringOrNull()
        );

        public PagesExtractor? TryGetPage() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("page")?
             .Pipe(i => new PagesExtractor(i))
        );

        private JsonElement? TryGetArc() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("arc")
        );

        public string? TryGetThumbnail() => Memory.Cache(this, () =>
             TryGetArc()?
             .GetPropertyOrNull("pic")?
             .GetStringOrNull()
        );

        public string? TryGetTitle() => Memory.Cache(this, () =>
             TryGetArc()?
             .GetPropertyOrNull("title")?
             .GetStringOrNull()
        );

        public int? TryGetCid() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("cid")?
             .GetInt32OrNull()
        );

        public TimeSpan? TryGetDuration() => Memory.Cache(this, () =>
             TryGetArc()?
             .GetPropertyOrNull("duration")?
             .GetInt32OrNull()?
             .TryConvertDoubleOrNull()?
             .Pipe(TimeSpan.FromSeconds)
        );
    }
}
