using BiliDownloader.Core.Controllers;
using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
using BiliDownloader.Core.Videos.Pages;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    internal class StreamController(HttpClient httpClient) : BaseController(httpClient)
    {
        public async ValueTask<StreamInfoExtractor> GetManifestAsync(IPlaylist playlist, CancellationToken cancellationToken)
        {
            var raw = await SendHttpRequestAsync(playlist.Url, cancellationToken);
            StreamInfoExtractor streamExtractor = StreamInfoExtractor.TryCreate(raw);
            if (!streamExtractor.IsStreamAvailable())
            {
                throw new DownloaderException($"获取下载地址失败");
            }
            return streamExtractor;
        }
    }
}
