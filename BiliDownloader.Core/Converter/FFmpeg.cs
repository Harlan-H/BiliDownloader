using BiliDownloader.Core.Utils.Extensions;
using CliWrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Converter
{
    public partial class FFmpeg
    {
        private readonly string _filePath;

        public FFmpeg(string filePath) => _filePath = filePath;

        public async ValueTask ExecuteAsync(
             string arguments,
             CancellationToken cancellationToken = default)
        {
            var stdErrBuffer = new StringBuilder();

            var stdErrPipe = PipeTarget.Merge(
                PipeTarget.ToStringBuilder(stdErrBuffer), // error data collector
                PipeTarget.Null // progress
            );

            var result = await Cli.Wrap(_filePath)
                .WithArguments(arguments)
                .WithStandardErrorPipe(stdErrPipe)
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync(cancellationToken);

            if (result.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"ffmpeg异常退出 退出码 ({result.ExitCode})." +
                    Environment.NewLine +

                    "参数:" +
                    Environment.NewLine +
                    arguments +
                    Environment.NewLine +

                    "错误是:" +
                    Environment.NewLine +
                    stdErrBuffer
                );
            }
        }
    }
}
