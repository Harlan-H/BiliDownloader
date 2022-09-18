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
    public class ClosedCaptionTraceInfoExtractor
    {
        private readonly JsonElement jsonElement;

        public ClosedCaptionTraceInfoExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public string? TryGetUrl() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("subtitle_url")?
            .GetStringOrNull()
        );

        public string? TryGetLanguageCode() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("lan")?
            .GetStringOrNull()
        );

        public string? TryGetLanguageName() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("lan_doc")?
            .GetStringOrNull()
        );
    }
}
