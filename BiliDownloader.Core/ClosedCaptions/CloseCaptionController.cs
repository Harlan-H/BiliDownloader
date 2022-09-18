using BiliDownloader.Core.Controllers;
using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    internal class CloseCaptionController : BaseController
    {
        public CloseCaptionController(HttpClient httpClient) : base(httpClient)
        {
        }
         
        public async ValueTask<ClosedCaptionResponseExtractor> GetClosedCaptionResponseAsync(IPlaylist playlist,CancellationToken cancellationToken = default)
        {
            var content = await SendHttpRequestAsync(playlist.ClosedCaptionUrl, cancellationToken);
            ClosedCaptionResponseExtractor closedCaptionTraceExtractor = ClosedCaptionResponseExtractor.TryCreate(content);
            if (!closedCaptionTraceExtractor.IsSubtitleAvailable())
            {
                throw new DownloaderException($"下载字幕失败,字幕地址{playlist.ClosedCaptionUrl}");
            }
            return closedCaptionTraceExtractor;
        }

        public async ValueTask<ClosedCaptionTraceExtractor> GetClosedCaptions(ClosedCaptionTrackInfo trackInfo ,CancellationToken cancellationToken)
        {
            var content = await SendHttpRequestAsync(trackInfo.Url.OriginalString, cancellationToken);
            return ClosedCaptionTraceExtractor.TryCreate(content);
        }
    }
}
