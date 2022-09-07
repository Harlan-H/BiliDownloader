using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BiliDownloader.Core.Extractors
{
    internal class SectionPagesExtractor
    {
        private readonly JsonElement jsonElement;
        public SectionPagesExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public IReadOnlyList<EpisodesExtractor> TryGetEpisodes() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("episodes")?
             .EnumerateArrayOrNull()?
             .Select(i => new EpisodesExtractor(i))
             .ToArray() ??
             Array.Empty<EpisodesExtractor>()
        );
    }
}
