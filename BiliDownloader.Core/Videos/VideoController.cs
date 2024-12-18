using BiliDownloader.Core.Controllers;
using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos
{
    internal class VideoController(HttpClient httpClient) : BaseController(httpClient)
    {
        public async ValueTask<VideoPageExtractor> GetVideoPageAsync(
            VideoId videoid,
            CancellationToken cancellationToken)
        {
            var raw = await SendHttpRequestAsync(videoid.Url, cancellationToken);
            var videoExtractor = VideoPageExtractor.TryCreate(raw);
            if (videoExtractor == null)
            {
                throw new DownloaderException($"请求地址是无效的");
            }

            return videoExtractor;
        }

    }
}
