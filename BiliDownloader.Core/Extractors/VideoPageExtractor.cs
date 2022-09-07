using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BiliDownloader.Core.Extractors
{
    internal partial class VideoPageExtractor
    {
        private readonly JsonElement jsonElement;

        public VideoPageExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        private JsonElement? TryGetSectionsInfo() => Memory.Cache(this, () =>jsonElement.GetPropertyOrNull("sectionsInfo"));

        private JsonElement? TryGetVideoData() => Memory.Cache(this, () => jsonElement.GetPropertyOrNull("videoData"));

        private JsonElement? TryGetOwner() => Memory.Cache(this, () => TryGetVideoData()?.GetPropertyOrNull("owner"));

        public string? TryGetVideoTitle() => Memory.Cache(this, () =>
            TryGetVideoData()?
            .GetPropertyOrNull("title")?
            .GetStringOrNull()
        );

        public string? TryGetAuthroName() => Memory.Cache(this, () =>
             TryGetOwner()?
             .GetPropertyOrNull("name")?
             .GetStringOrNull()
        );

        public string? TryGetDescription() => Memory.Cache(this, () =>
             TryGetVideoData()?
             .GetPropertyOrNull("desc")?
             .GetStringOrNull()
        );

        public TimeSpan? TryGetDuration() => Memory.Cache(this, () =>
             TryGetVideoData()?
              .GetPropertyOrNull("duration")?
              .GetInt32OrNull()?
              .TryConvertDoubleOrNull()?
              .Pipe(TimeSpan.FromSeconds)
        );

        public string? TryGetThumbnail() => Memory.Cache(this, () =>
             TryGetVideoData()?
             .GetPropertyOrNull("pic")?
             .GetStringOrNull()
        );

        public IReadOnlyList<PagesExtractor> TryGetPages() => Memory.Cache(this, () =>
             TryGetVideoData()?
            .GetPropertyOrNull("pages")?
            .EnumerateArrayOrNull()?
            .Select( i => new PagesExtractor(i))
            .ToArray() ??
            Array.Empty<PagesExtractor>()
        );

        public IReadOnlyList<EpisodesExtractor> TryGetEpisodes() => Memory.Cache(this, () =>
             TryGetSectionsInfo()?
            .GetPropertyOrNull("sections")?
            .EnumerateArrayOrNull()?
            .Select(i => new SectionPagesExtractor(i))
            .SelectMany(p => p.TryGetEpisodes())
            .ToArray() ??
            Array.Empty<EpisodesExtractor>()
        );
    }

    internal partial class VideoPageExtractor
    {
        private static readonly Regex regex = new(@"window\.__INITIAL_STATE__=([\s|\S]+);\(function", RegexOptions.Compiled);
        public static VideoPageExtractor? TryCreate(string raw)
        {
            var InitState = regex.Match(raw).Groups[1].Value;
            if (string.IsNullOrWhiteSpace(InitState))
                return null;

            return new VideoPageExtractor(Json.Parse(InitState));
        }
    }
}
