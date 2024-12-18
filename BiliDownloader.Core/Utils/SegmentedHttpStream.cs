using BiliDownloader.Core.Utils.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils
{
    internal class SegmentedHttpStream(HttpClient httpClient, string url, long filesize) : Stream
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly string url = url;

        private Stream? _segmentStream;
        private long _actualPosition;
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length { get; } = filesize;

        public override long Position { get; set; }

        private void ResetSegmentStream()
        {
            _segmentStream?.Dispose();
            _segmentStream = null;
        }

        private async ValueTask<Stream> ResolveSegmentStreamAsync(CancellationToken cancellationToken = default)
        {
            if (_segmentStream is not null)
                return _segmentStream;

            var stream = await httpClient.GetStreamAsync(url, true, cancellationToken);

            return _segmentStream = stream;
        }

        public async ValueTask PreloadAsync(CancellationToken cancellationToken = default) => await ResolveSegmentStreamAsync(cancellationToken);

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                if (_actualPosition != Position)
                    ResetSegmentStream();

                if (Position >= Length)
                    return 0;

                var stream = await ResolveSegmentStreamAsync(cancellationToken);
                var bytesRead = await stream.ReadAsync(buffer, cancellationToken);
                _actualPosition = Position += bytesRead;

                if (bytesRead != 0)
                    return bytesRead;

                ResetSegmentStream();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return Position = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
