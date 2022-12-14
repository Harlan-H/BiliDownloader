using BiliDownloader.Core;
using BiliDownloader.Core.ClosedCaptions;
using BiliDownloader.Core.Videos.Pages;
using BiliDownloader.Core.Videos.Streams;
using BiliDownloader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Services
{
    public class DownloadService : IDisposable
    {
        private readonly BiliDownloaderClient biliDownloaderClient = new(Http.Client);
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly SettingsService settingsService;
        private int _concurrentDownloadCount;

        public DownloadService(SettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        private async Task EnsureThrottlingAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                while (_concurrentDownloadCount >= settingsService.MaxConcurrentDownloadCount)
                    await Task.Delay(1000, cancellationToken);

                Interlocked.Increment(ref _concurrentDownloadCount);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DownloadAsync(IPlaylist playlist, string filePath, Func<long,DownloadRate> func, CancellationToken cancellationToken = default)
        {
            await EnsureThrottlingAsync(cancellationToken);

            try
            {
                var streamManifest = await biliDownloaderClient.Videos.Streams.GetStreamManifestAsync(playlist, cancellationToken);

                var streamInfos = new List<StreamInput>();
                await biliDownloaderClient.Videos.Streams.GetVideoDownloadOptionsAsync(streamManifest, filePath, streamInfos, cancellationToken);

                foreach (var streamInfo in streamInfos)
                {
                    using DownloadRate downloadrate = func(streamInfo.Info.FileSize);
                    downloadrate.Run();
                    await biliDownloaderClient.Videos.Streams.DownloadAsync(streamInfo.Info, streamInfo.FilePath, downloadrate, cancellationToken);
                }

                if(settingsService.DownloadSubtitle)
                {
                    var closedCaptionResponse = await biliDownloaderClient.Videos.ClosedCaptions.GetClosedCaptionManifestAsync(playlist, cancellationToken);

                    if(closedCaptionResponse.TrackInfos.Any())
                    {
                        var subtitles = new List<ClosedCaptionInput>(closedCaptionResponse.TrackInfos.Count);
                        biliDownloaderClient.Videos.ClosedCaptions.PopulateSubtitleInputsAsync(closedCaptionResponse, filePath, subtitles);
                        foreach (var subtitle in subtitles)
                        {
                            await biliDownloaderClient.Videos.ClosedCaptions.DownloadAsync(subtitle.TrackInfo, subtitle.FilePath, null, cancellationToken);
                        }
                    }
                }

                //ffmpeg合并文件
                await BiliDownloaderClient.Converter(streamInfos, filePath, cancellationToken);
            }
            finally
            {
                Interlocked.Decrement(ref _concurrentDownloadCount);
            }
        }

        public void Dispose()
        {
            _semaphore.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
