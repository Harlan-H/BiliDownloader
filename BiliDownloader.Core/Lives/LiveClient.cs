using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Lives
{
    public class LiveClient
    {
        private readonly LiveController liveController;
        public LiveClient(HttpClient httpClient)
        {
            liveController = new(httpClient);
        }

        public async ValueTask<LiveInfo> GetVideoInfoAsync(LiveId liveId, CancellationToken cancellationToken = default)
        {
            var videoExtractor = await liveController.GetVideoPageAsync(liveId, cancellationToken);

            return LiveExtract(videoExtractor, liveId);
        }


        public async ValueTask<LiveInfo> GetVideoInfoAsync(FileInfo fileInfo, LiveId videoId)
        {
            var videoExtractor = await liveController.GetVideoPageAsync(fileInfo);

            return LiveExtract(videoExtractor, videoId);
        }

        private static LiveInfo LiveExtract(LivePageExtractor livePageExtractor, LiveId liveId)
        {
            var livestatus = livePageExtractor.TryGetLiveStatus() ?? throw new DownloaderException("获取直播状态失败");
            if (livestatus != 1)
                throw new DownloaderException("主播没有开播");

            var title = livePageExtractor.TryGetLiveTitle() ?? throw new DownloaderException("获取标题失败");
            var description = livePageExtractor.TryGetDescription() ?? "";
            var author = livePageExtractor.TryGetAuthor();
          //  var starttime = livePageExtractor.TryGetLiveStartTime() ?? TimeSpan.Zero;
            var thumb = livePageExtractor.TryGetLiveThumbnail();

            IList<IPlaylist> livePlayList = new List<IPlaylist>() { new LivePlayList(liveId,title) };
            return new LiveInfo(title, description, new Common.Author(author), TimeSpan.Zero, thumb, livePlayList);
        }
    }
}
