using BiliDownloader.Core;
using BiliDownloader.Core.Videos;
using BiliDownloader.Models;
using BiliDownloader.Utils;
using System;
using System.Threading.Tasks;

namespace BiliDownloader.Services
{
    public class QueryService
    {
        private readonly BiliDownloaderClient biliDownloaderClient = new(Http.Client);
        public async Task<QueryModel> ParseQuery(string? url)
        {
            url = url?.Trim();

            var videoId = VideoId.TryParse(url);
            if(videoId != null)
            {
                var video = await biliDownloaderClient.Videos.GetVideoInfoAsync(videoId.Value);
                return new QueryModel(videoId, video);
            }

            throw new InvalidOperationException("不支持得请求地址");
        }

    }
}
