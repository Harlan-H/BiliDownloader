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
    internal partial class ClosedCaptionResponseExtractor
    {
        private readonly JsonElement jsonElement;

        public ClosedCaptionResponseExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public bool IsSubtitleAvailable() => Memory.Cache(this, () =>
            jsonElement
            .GetProperty("code")
            .GetInt32() == 0
        );

        public IReadOnlyList<ClosedCaptionTraceInfoExtractor> TryCloseCaptionTrace() => Memory.Cache(this, () =>
             jsonElement
            .GetPropertyOrNull("data")?
            .GetPropertyOrNull("subtitle")?
            .GetPropertyOrNull("subtitles")?
            .EnumerateArrayOrNull()?
            .Select(i => new ClosedCaptionTraceInfoExtractor(i))
            .ToArray() ??
            Array.Empty<ClosedCaptionTraceInfoExtractor>()
        );
    }

    internal partial class ClosedCaptionResponseExtractor
    {
        public static ClosedCaptionResponseExtractor TryCreate(string text) => new(Json.Parse(text));
    }
}
