using BiliDownloader.Core.Utils.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils
{
    internal class SegmentedHttpStream : Stream
    {
        private readonly HttpClient httpClient;
        private readonly string url;

        private Stream? _segmentStream;
        private long _actualPosition;
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length { get; }

        public override long Position { get; set; }

        public SegmentedHttpStream(HttpClient httpClient, string url, long filesize)
        {
            this.httpClient = httpClient;
            this.url = url;
            Length = filesize;
        }

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
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (_actualPosition != Position)
                    ResetSegmentStream();

                //这里必须干掉这个比较 不然会出现一个bug  在直播下载的时候 是没有长度的 也不知道长度 就应该一直保持连接 一直到中断
                if (Length != 0 && Position >= Length)
                    return 0;

                var stream = await ResolveSegmentStreamAsync(cancellationToken);
                var bytesRead = await stream.ReadAsync(buffer, offset, count, cancellationToken);
                _actualPosition = Position += bytesRead;

                if (bytesRead != 0)
                    return bytesRead;

                ResetSegmentStream();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return ReadAsync(buffer, offset, count).GetAwaiter().GetResult();
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
