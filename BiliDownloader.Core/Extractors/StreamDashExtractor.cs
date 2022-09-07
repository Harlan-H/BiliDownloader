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
    internal class StreamDashExtractor : IStreamInfoExtractor
    {
        private readonly JsonElement jsonElement;

        public bool IsVideoStream => TryGetDashType() == "video/mp4";

        public StreamDashExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public int? TryGetFileSize() => null;

        public string? TryGetUrl() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("baseUrl")?
             .GetStringOrNull()
        );

        public int? TryGetQuality() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("id")?
            .GetInt32OrNull()
        );

        public int? TryGetBandWidth() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("bandwidth")?
            .GetInt32OrNull()
        );

        public string? TryGetCodec() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("codecs")?
            .GetStringOrNull()
        );

        public string? TryGetDashType() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("mimeType")?
            .GetStringOrNull()
        );


    }
}
