using BiliDownloader.Core.Controllers;
using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
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
    internal class LiveController : BaseController
    {
        public LiveController(HttpClient httpClient) : base(httpClient)
        {

        }

        public async ValueTask<LivePageExtractor> GetVideoPageAsync(
            LiveId liveId,
            CancellationToken cancellationToken)
        {
            using HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, liveId.Url);
            if(!httpRequestMessage.Headers.Contains("accept"))
            {
                httpRequestMessage.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            }
            var raw = await SendHttpRequestAsync(httpRequestMessage, cancellationToken);
            var liveExtractor = LivePageExtractor.TryCreate(raw);
            if (liveExtractor == null)
            {
                throw new DownloaderException($"请求地址是无效的");
            }

            return liveExtractor;
        }

        public async ValueTask<LivePageExtractor> GetVideoPageAsync(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
                return null!;

            var raw = await fileInfo.OpenText().ReadToEndAsync();
            var liveExtractor = LivePageExtractor.TryCreate(raw);
            if (liveExtractor == null)
            {
                throw new DownloaderException($"请求地址是无效的");
            }

            return liveExtractor;
        }
    }
}
