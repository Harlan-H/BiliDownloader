using BiliDownloader.Core.Exceptions;
using BiliDownloader.Core.Extractors;
using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class StreamClient
    {
        private readonly StreamController streamController;
        private readonly HttpClient httpClient;

        public StreamClient(HttpClient httpClient)
        {
            streamController = new StreamController(httpClient);
            this.httpClient = httpClient;
        }



        private void PopulateStreamInfosAsync(
            ICollection<IStreamInfo> streamInfos,
            IEnumerable<IStreamInfoExtractor> streamInfoExtractors)
        {
            foreach (var streamInfoExtractor in streamInfoExtractors)
            {
                string url = streamInfoExtractor.TryGetUrl() ??
                    throw new DownloaderException("获取下载地址失败"); ;

                var contentLength = streamInfoExtractor.TryGetFileSize() ?? 0;

                var fileSize = new FileSize(contentLength);

                var bandwidth = streamInfoExtractor.TryGetBandWidth() ?? 0;
                var codecs = streamInfoExtractor.TryGetCodec() ?? "";
                var quality = streamInfoExtractor.TryGetQuality() ?? 0;

                //当bandwidth和quality同时为0说明是直播数据 也就是混合流
                if (bandwidth == 0 && quality == 0)
                {
                    var muxedStreamInfo = new MuxedStreamInfo(url, fileSize);
                    streamInfos.Add(muxedStreamInfo);
                }
                else
                {
                    //这里当类型是video/mp4的时候是视频流 否则是音频流
                    if(streamInfoExtractor.IsVideoStream)
                    {
                        var videoStreamInfo = new VideoOnlyStreamInfo(url, quality, codecs, fileSize, bandwidth);
                        streamInfos.Add(videoStreamInfo);
                    }
                    else
                    {
                        var audioStreamInfo = new AudioOnlyStreamInfo(url, quality, codecs, fileSize, bandwidth);
                        streamInfos.Add(audioStreamInfo);
                    }
                }
            }
        }

        public async ValueTask<StreamManifest> GetStreamManifestAsync(
           IPlaylist playlist,
           CancellationToken cancellationToken)
        {
            var streamResult = await streamController.GetManifestAsync(playlist, cancellationToken);

            if (!streamResult.IsStreamAvailable())
                throw new DownloaderException("获取视频流出现错误");

            var streamInfos = new List<IStreamInfo>();

            PopulateStreamInfosAsync(streamInfos, streamResult.TryGetStreamExtractor());

            return new StreamManifest(streamInfos);
        }


        public async ValueTask GetVideoDownloadOptionsAsync(StreamManifest streamManifest,string filePath, ICollection<StreamInput> streamInputs, CancellationToken cancellationToken = default)
        {
            if (streamManifest.GetAudioOnlyStreams().Any() && streamManifest.GetVideoOnlyStreams().Any())
            {
                var videoInfo = streamManifest
                                .GetVideoOnlyStreams()
                                .OrderByDescending(o => o.VideoQuality)
                                .ThenByDescending(o => o.BandWidth)
                                .FirstOrDefault();

                if (videoInfo is null)
                    throw new InvalidOperationException("没有任何视频数据");

                {

                    long contentLength = videoInfo.FileSize.Bytes == 0
                        ? await httpClient.TryGetContentLengthAsync(videoInfo.Url, false, cancellationToken)
                        ?? throw new InvalidDataException("获取视频文件大小失败，请稍等几分钟后重试")
                        : videoInfo.FileSize.Bytes;

                    videoInfo.UpdateFileSize(new FileSize(contentLength));

                    string videoPath = $"{filePath}.video.tmp";
                    StreamInput streamInput = new(videoInfo, videoPath);
                    streamInputs.Add(streamInput);
                }



                var audioInfo = streamManifest
                                .GetAudioOnlyStreams()
                                .OrderByDescending(o => o.AudioQuality)
                                .ThenByDescending(o => o.BandWidth)
                                .FirstOrDefault();

                if (audioInfo is null)
                    throw new InvalidOperationException("没有任何音频数据");

                {
                    long contentLength = audioInfo.FileSize.Bytes == 0
                        ? await httpClient.TryGetContentLengthAsync(audioInfo.Url, false, cancellationToken)
                        ?? throw new InvalidDataException("获取音频文件大小失败，请稍等几分钟后重试")
                        : audioInfo.FileSize.Bytes;

                    audioInfo.UpdateFileSize(new FileSize(contentLength));

                    string audioPath = $"{filePath}.audio.tmp";
                    StreamInput streamInput = new(audioInfo, audioPath);
                    streamInputs.Add(streamInput);
                }

            }
            else
            {
                var muxedInfo = streamManifest
                            .GetMuxedStreams()
                            .FirstOrDefault();
                if (muxedInfo is not null)
                {
                    string muxedPath = $"{filePath}-{DateTime.Now:MMddHHmmss}";
                    StreamInput streamInput = new(muxedInfo, muxedPath);
                    streamInputs.Add(streamInput);
                }
            }

            if (!streamInputs.Any())
                throw new InvalidOperationException("没有任何下载数据");
        }

        public async ValueTask<Stream> GetAsync(IStreamInfo streamInfo, CancellationToken cancellationToken = default)
        {
            var stream = new SegmentedHttpStream(httpClient, streamInfo.Url, streamInfo.FileSize);

            await stream.PreloadAsync(cancellationToken);

            return stream;
        }

        public async ValueTask CopyToAsync(
            IStreamInfo streamInfo,
            Stream destination,
            IProgress<long>? updateCallback = null,
            CancellationToken cancellationToken = default)
        {
            using var input = await GetAsync(streamInfo, cancellationToken);
            await input.CopyToAsync(destination, updateCallback, cancellationToken);
        }

        public async ValueTask DownloadAsync(
            IStreamInfo streamInfo,
            string filePath,
            IProgress<long>? updateCallback = null,
            CancellationToken cancellationToken = default)
        {
            using var destination = File.Create(filePath);
            await CopyToAsync(streamInfo, destination, updateCallback, cancellationToken);
        }
    }
}
