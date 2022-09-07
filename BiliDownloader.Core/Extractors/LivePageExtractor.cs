using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Extractors
{
    internal partial class LivePageExtractor
    {
        private readonly JsonElement jsonElement;

        public LivePageExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        private JsonElement? TryGetRoomInfoData() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("roomInfoRes")?
            .GetPropertyOrNull("data")
        );

        public bool IsLiveDataAvailable() => Memory.Cache(this, () =>
            TryGetRoomInfoData()?
            .GetPropertyOrNull("code")?
            .GetInt32OrNull() == 0
        );

        private JsonElement? TryGetRoomInfo() => Memory.Cache(this, () =>
             TryGetRoomInfoData()?
             .GetPropertyOrNull("room_info")
        );

        public int? TryGetRoomId() => Memory.Cache(this, () =>
            TryGetRoomInfo()?
            .GetPropertyOrNull("room_id")?
            .GetInt32OrNull()
        );

        // 1直播  2离线
        public int? TryGetLiveStatus() => Memory.Cache(this, () =>
              TryGetRoomInfo()?
              .GetPropertyOrNull("live_status")?
              .GetInt32OrNull()
         );

        public TimeSpan? TryGetLiveStartTime() => Memory.Cache(this, () =>
              TryGetRoomInfo()?
              .GetPropertyOrNull("live_start_time")?
              .GetInt32OrNull()?
              .TryConvertDoubleOrNull()?
               .Pipe(TimeSpan.FromSeconds) 
        );

        public string? TryGetLiveTitle() => Memory.Cache(this, () =>
              TryGetRoomInfo()?
              .GetPropertyOrNull("title")?
              .GetStringOrNull()
        );

        public string? TryGetLiveThumbnail() => Memory.Cache(this, () =>
              TryGetRoomInfo()?
              .GetPropertyOrNull("cover")?
              .GetStringOrNull()
        );

        public string? TryGetDescription() => Memory.Cache(this, () =>
            TryGetRoomInfo()?
            .GetPropertyOrNull("description")?
            .GetStringOrNull()
        );

        public string? TryGetAuthor() => Memory.Cache(this, () =>
             TryGetRoomInfoData()?
             .GetPropertyOrNull("anchor_info")?
             .GetPropertyOrNull("base_info")?
             .GetPropertyOrNull("uname")?
             .GetStringOrNull()
        );


    }

    internal partial class LivePageExtractor
    {
        private static readonly Regex regex = new(@"__NEPTUNE_IS_MY_WAIFU__=(\{.*?\})</script>", RegexOptions.Compiled);
        public static LivePageExtractor? TryCreate(string raw)
        {
            var initState = regex.Match(raw).Groups[1].Value;
            if (string.IsNullOrWhiteSpace(initState))
                return null;

            return new LivePageExtractor(Json.Parse(initState));
        }
    }
}
