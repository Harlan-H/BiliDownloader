using BiliDownloader.Core;
using BiliDownloader.Core.Lives;
using BiliDownloader.Core.Videos;
using BiliDownloader.Models;
using BiliDownloader.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BiliDownloader.Services
{
    public class QueryService
    {
        private readonly BiliDownloaderClient biliDownloaderClient = new(Http.Client);
        public async Task<QueryModel> ParseQuery(string? url)
        {
            url = url?.Trim();

//             var liveid = LiveId.TryParse(url);
//             if(liveid != null)
//             {
//                 var video = await biliDownloaderClient.Lives.GetVideoInfoAsync(liveid.Value);
//                 return new QueryModel(liveid, video);
//             }

            var videoId = VideoId.TryParse(url);
            if(videoId != null)
            {
                var video = await biliDownloaderClient.Videos.GetVideoInfoAsync(videoId.Value);
                return new QueryModel(videoId, video);
            }

            throw new InvalidOperationException("不支持得请求地址");
        }

        public async Task<IVideo> ParseQuery(FileInfo fileInfo, string uid)
        {
            if (!fileInfo.Exists)
                throw new FileNotFoundException("文件不存在");

            return await biliDownloaderClient.Videos.GetVideoInfoAsync(fileInfo, uid);
        }
    }
}
