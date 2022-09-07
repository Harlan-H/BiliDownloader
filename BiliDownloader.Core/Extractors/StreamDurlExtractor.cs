using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using BiliDownloader.Core.Videos.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BiliDownloader.Core.Extractors
{
    internal class StreamDurlExtractor : IStreamInfoExtractor
    {
        private readonly JsonElement jsonElement;

        public bool IsVideoStream { get; } = true;

        public StreamDurlExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public int? TryGetBandWidth() => null;

        public string? TryGetCodec() => null;

        public int? TryGetQuality() => null;

        public string? TryGetDashType() => null;

        public int? TryGetFileSize() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("size")?
             .GetInt32OrNull()
        );

        public string? TryGetUrl() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("url")?
            .GetStringOrNull()
        );

        public IReadOnlyList<string> TryGetBackupUrl()
        {
            throw new System.NotImplementedException();
        }
    }
}
