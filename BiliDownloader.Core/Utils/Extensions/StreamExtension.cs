using System;
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
            Memory<byte> memory = new(buffer.Array);
            long totalBytes = 0L;
            int bytesCopied = 0;
            do
            {
                bytesCopied = await stream.ReadAsync(memory, cancellationToken);
                if (bytesCopied > 0) await destination.WriteAsync(memory[..bytesCopied], cancellationToken);

                totalBytes += bytesCopied;
                updateCallback?.Report(totalBytes);
            } while (bytesCopied > 0);
        }
    }
}
