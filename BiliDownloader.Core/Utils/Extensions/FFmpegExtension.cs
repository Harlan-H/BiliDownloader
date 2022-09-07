using BiliDownloader.Core.Converter;
using BiliDownloader.Core.Videos.Streams;
using CliWrap.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    public static class FFmpegExtension
    {
        public static async ValueTask Convert(this FFmpeg ffmpeg, IEnumerable<StreamInput> streamInputs, string outputFile, CancellationToken cancellationToken = default)
        {
            var arguments = new ArgumentsBuilder();

            foreach (var streamInput in streamInputs)
            {
                arguments.Add("-i").Add(streamInput.FilePath);
            }

            arguments.Add("-f").Add("mp4");

            arguments
                .Add("-c:a").Add("copy")
                .Add("-c:v").Add("copy");

            var tmpoutputFile = Path.ChangeExtension(outputFile, "mp4");
            arguments
                .Add("-nostdin")
                .Add("-y").Add(tmpoutputFile);

            await ffmpeg.ExecuteAsync(arguments.Build(), cancellationToken);

            foreach (var streamInput in streamInputs)
            {
                streamInput.Dispose();
            }

        }
    }
}
