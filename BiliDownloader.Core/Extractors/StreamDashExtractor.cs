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

        //使用backupUrl的请求地址，baseUrl的地址可能回出现404的问题
        public string? TryGetUrl() => Memory.Cache(this, () =>
        {
            var backUpUrlArr = jsonElement
            .GetPropertyOrNull("backupUrl")?
            .EnumerateArrayOrNull()?
            .Select(i => i.GetStringOrNull())?
            .ToArray();

            if(backUpUrlArr is not null && backUpUrlArr.Any())
            {
                
                int index = Random.Shared.Next(backUpUrlArr.Length);
                return backUpUrlArr[index];
            }
            return null;
         }
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
