using BiliDownloader.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Pages
{
    public class PlayList(
        VideoId videoId,
        long aid,
        long cid,
        string? title,
        TimeSpan duration
            ) : IPlaylist
    {
        public VideoId Id { get; } = videoId;
        private long Aid { get; } = aid;
        private long Cid { get; } = cid;
        private static string Session => System.Guid.NewGuid().ToString("N");

        //https://api.bilibili.com/x/player/wbi/playurl?avid=1956464087&bvid=BV1Xy411e7PY&cid=1635270299&qn=80&fnver=0&fnval=4048&fourk=1&gaia_source=&from_client=BROWSER&is_main_page=true&need_fragment=false&isGaiaAvoided=false&session=3a502f8579bdc75faf2f1a52d7928a2c&voice_balance=1&web_location=1315873&w_rid=8dec08e35283e19bb13009a6081181db&wts=1725090465
        public string Url => $"https://api.bilibili.com/x/player/wbi/playurl?avid={Aid}&bvid={Id}&cid={Cid}&qn=112&fnver=0&type=&otype=json&fourk=1&fnval=4048&gaia_source=&from_client=BROWSER&is_main_page=true&need_fragment=false&isGaiaAvoided=false&session={Session}&voice_balance=1&web_location=1315873&w_rid={Session}&wts={DateTimeOffset.Now.ToUnixTimeSeconds()}";

        //https://api.bilibili.com/x/player/wbi/v2?aid=112909606716397&cid=500001639372429&isGaiaAvoided=false&web_location=1315873&w_rid=c186b4d645b33256e9b13a7084c203be&wts=1725094333
        public string ClosedCaptionUrl => $"https://api.bilibili.com/x/player/wbi/v2?aid={Aid}&cid={Cid}";
        public string? Title { get; } = title;
        public TimeSpan? Duration { get; } = duration;
    }
}
