using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils
{
    internal readonly struct PooledBuffer<T> : IDisposable
    {
        public T[] Array { get; }
        public PooledBuffer(int minlength) => Array = ArrayPool<T>.Shared.Rent(minlength);
        public void Dispose() => ArrayPool<T>.Shared.Return(Array);
    }

    internal static class PooledBuffer
    {
        public static PooledBuffer<byte> GetStream() => new(81920);
    }
}
