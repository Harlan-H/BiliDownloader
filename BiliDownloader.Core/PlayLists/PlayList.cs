using BiliDownloader.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Pages
{
    public class PlayList : IPlaylist
    {
        public VideoId Id { get; }
        private int Aid { get; }
        private int Cid { get; }
        private static string Session => System.Guid.NewGuid().ToString("N");

        //https://api.bilibili.com/x/player/playurl?cid=559618300&bvid=BV1pY4y1s7Wv&qn=112&fnval=0&session=275dc198aea149bf918e3f5b51a5e508
        //https://api.bilibili.com/x/player/playurl?cid=578434284&bvid=BV1jS4y1e7rj&qn=112&fnval=4048&session=275dc198aea149bf918e3f5b51a5e508
        public string Url => $"http://api.bilibili.com/x/player/playurl?cid={Cid}&bvid={Id}&qn=112&type=&otype=json&fourk=1&fnval=4048&session={Session}";
        public string ClosedCaptionUrl => $"https://api.bilibili.com/x/player/v2?aid={Aid}&cid={Cid}";
        public string? Title { get; }
        public TimeSpan? Duration { get; }

        public PlayList(
            VideoId videoId,
            int aid,
            int cid,
            string? title,
            TimeSpan  duration
            )
        {
            Id = videoId;
            Aid = aid;
            Cid = cid;
            Title = title;
            Duration = duration;
        }

    }
}
