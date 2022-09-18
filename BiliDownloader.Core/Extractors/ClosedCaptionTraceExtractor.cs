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
    internal partial class ClosedCaptionTraceExtractor
    {
        private readonly JsonElement jsonElement;

        public ClosedCaptionTraceExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public IReadOnlyList<ClosedCaptionExtractor> TryClosedCaptionExtractor() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("body")?
            .EnumerateArrayOrNull()?
            .Select(i => new ClosedCaptionExtractor(i))
            .ToArray()??
            Array.Empty<ClosedCaptionExtractor>()
        );
    }

    internal partial class ClosedCaptionTraceExtractor
    {
        public static ClosedCaptionTraceExtractor TryCreate(string text) => new(Json.Parse(text));
    }
}
