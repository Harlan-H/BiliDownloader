using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    public static class StreamExtension
    {
        public static async ValueTask CopyToAsync(this Stream stream,Stream destination, IProgress<long>? updateCallback, CancellationToken cancellationToken = default)
        {
            using var buffer = PooledBuffer.GetStream();
            long totalBytes = 0L;
            int bytesCopied = 0;
            do
            {
                bytesCopied = await stream.ReadAsync(buffer.Array, cancellationToken);
                if (bytesCopied > 0) await destination.WriteAsync(buffer.Array, cancellationToken);

                totalBytes += bytesCopied;
                updateCallback?.Report(totalBytes);
            } while (bytesCopied > 0);
        }
    }
}
