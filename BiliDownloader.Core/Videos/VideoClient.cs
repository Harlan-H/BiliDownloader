using BiliDownloader.Core.ClosedCaptions;
using BiliDownloader.Core.Common;
using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
using BiliDownloader.Core.Videos.Pages;
using BiliDownloader.Core.Videos.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos
{
    public class VideoClient
    {
        private readonly VideoController videoController;
        public StreamClient Streams { get; }
        public ClosedCaptionClient ClosedCaptions { get; }

        public VideoClient(HttpClient httpClient)
        {
            videoController = new VideoController(httpClient);
            Streams = new StreamClient(httpClient);
            ClosedCaptions = new ClosedCaptionClient(httpClient);
        }

        public VideoClient() : this(new HttpClient())
        {

        }

        public async ValueTask<Video> GetVideoInfoAsync(VideoId videoId,CancellationToken cancellationToken = default)
        {
            var videoExtractor = await videoController.GetVideoPageAsync(videoId, cancellationToken);
            return VideoExtract(videoExtractor, videoId);
        }


        private static Video VideoExtract(VideoPageExtractor videoExtractor, VideoId videoId)
        {
            var aid = videoExtractor.TryGetAid() ?? 0;
            var title = videoExtractor.TryGetVideoTitle() ?? throw new DownloaderException("提取标题失败");
            var description = videoExtractor.TryGetDescription()!;
            var author = videoExtractor.TryGetAuthroName();
            var duration = videoExtractor.TryGetDuration() ?? TimeSpan.Zero;
            var thumbnail = videoExtractor.TryGetThumbnail();


            var authorTmp = new Author(author);
            var playlists = PlayListHelper.GetPlayLists(videoExtractor) ??
                PlayListHelper.GetPlayLists(videoExtractor,aid, videoId) ??
                throw new DownloaderException("获取视频列表失败");

            return new Video(title, description, authorTmp, duration, thumbnail, playlists!);
        }
    }
    
}
