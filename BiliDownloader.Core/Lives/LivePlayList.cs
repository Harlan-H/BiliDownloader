using BiliDownloader.Core.Videos.Pages;
using System;

namespace BiliDownloader.Core.Lives
{
    public class LivePlayList : IPlaylist
    {
        private int Cid { get; }
        public string Url => $"http://api.live.bilibili.com/room/v1/Room/playUrl?cid={Cid}&qn=10000&platform=web";
        public string? Title { get; }
        public TimeSpan? Duration { get; }

        public string ClosedCaptionUrl { get; } = default!;

        public LivePlayList(int cid, string? title = default, TimeSpan? timeSpan = default)
        {
            Cid = cid;
            Title = title;
            Duration = timeSpan;
        }
    }
}
