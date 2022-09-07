using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace BiliDownloader.Core.Extractors
{
    internal partial class StreamInfoExtractor
    {
        private readonly JsonElement jsonElement;

        public StreamInfoExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public bool IsStreamAvailable() => Memory.Cache(this, () =>
            jsonElement
            .GetProperty("code")
            .GetInt32() == 0
        );

        private JsonElement? GetData() => Memory.Cache(this, () =>
             jsonElement
             .GetPropertyOrNull("data")
         );



        public bool IsDashStream() => Memory.Cache(this, () =>
            GetData()?
            .GetPropertyOrNull("dash") != null
        );

        public IReadOnlyList<IStreamInfoExtractor> TryGetStreamExtractor() => Memory.Cache(this, () =>
        {

            var ret = GetData()?.GetPropertyOrNull("dash");
            if(ret != null)
            //if(IsDashStream())
            {
                var videoExtractors = GetData()?
                .GetPropertyOrNull("dash")?
                .GetPropertyOrNull("video")?
                .EnumerateArrayOrNull()?
                .Select(i => new StreamDashExtractor(i));

                var audioExtractors = GetData()?
                 .GetPropertyOrNull("dash")?
                 .GetPropertyOrNull("audio")?
                 .EnumerateArrayOrNull()?
                 .Select(i => new StreamDashExtractor(i));

                if (videoExtractors is not null && audioExtractors is not null)
                    return videoExtractors.Concat(audioExtractors).ToArray();

                return Array.Empty<IStreamInfoExtractor>(); 
            }
            else
            {
                 return GetData()?
                .GetPropertyOrNull("durl")?
                .EnumerateArrayOrNull()?
                .Select(i => new StreamDurlExtractor(i))
                .ToArray()??
                Array.Empty<IStreamInfoExtractor>();
            }
        });

    }


    internal partial class StreamInfoExtractor
    {
        public static StreamInfoExtractor TryCreate(string raw) => new(Json.Parse(raw));
    }
}
