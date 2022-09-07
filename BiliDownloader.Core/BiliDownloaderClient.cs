using BiliDownloader.Core.Converter;
using BiliDownloader.Core.Lives;
using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using BiliDownloader.Core.Videos;
using BiliDownloader.Core.Videos.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core
{
    public class BiliDownloaderClient
    {
#if DEBUG
        private const string FFmpegCliFilePath = @"F:\源代码\库\ffmpeg-4.3.1-2020-11-19-full_build-shared\bin\ffmpeg.exe";
#else
        private const string FFmpegCliFilePath = "./ffmpeg.exe";
#endif

        public VideoClient Videos { get; }

        //  public LiveClient Lives { get; }

        public BiliDownloaderClient(HttpClient httpClient)
        {
            Videos = new VideoClient(httpClient);
       //     Lives = new LiveClient(httpClient);
        }

        public BiliDownloaderClient() : this(Http.Client)
        {

        }

        public static async ValueTask Converter(IEnumerable<StreamInput> streamInputs, string outputFile, CancellationToken cancellationToken = default)
        {
            if (!streamInputs.Any()) return;

            FFmpeg fFmpeg = new(FFmpegCliFilePath);
            await fFmpeg.Convert(streamInputs, outputFile, cancellationToken);
        }
    }
}
