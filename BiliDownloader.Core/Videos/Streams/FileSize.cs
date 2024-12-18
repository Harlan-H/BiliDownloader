using System;

namespace BiliDownloader.Core.Videos.Streams
{
    public readonly partial struct FileSize(long bytes)
    {
        public long Bytes { get; } = bytes;

        public double KiloBytes => (double)Bytes / (1 << 10);

        public double MegaBytes => (double)Bytes / (1 << 20);

        public double GigaBytes => (double)Bytes / (1 << 30);

        private string GetLargestWholeNumberSymbol()
        {
            if (Math.Abs(GigaBytes) >= 1)
                return "GB";

            if (Math.Abs(MegaBytes) >= 1)
                return "MB";

            if (Math.Abs(KiloBytes) >= 1)
                return "KB";

            return "B";
        }

        private double GetLargestWholeNumberValue()
        {
            if (Math.Abs(GigaBytes) >= 1)
                return GigaBytes;

            if (Math.Abs(MegaBytes) >= 1)
                return MegaBytes;

            if (Math.Abs(KiloBytes) >= 1)
                return KiloBytes;

            return Bytes;
        }

        public override string ToString() => Bytes == 0 ? "∞" : $"{GetLargestWholeNumberValue():0.##} {GetLargestWholeNumberSymbol()}";
    }
    public partial struct FileSize : IEquatable<FileSize>
    {
        public bool Equals(FileSize other) => StringComparer.Ordinal.Equals(Bytes, other.Bytes);
        public override bool Equals(object? obj) => obj is FileSize other && Equals(other);
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Bytes);

        public static bool operator ==(FileSize left, FileSize right) => left.Equals(right);
        public static bool operator !=(FileSize left, FileSize right) => !(left == right);

        public static bool operator >(FileSize left, int right) => left.Bytes > right;
        public static bool operator <(FileSize left, int right) => left.Bytes < right;

        public static implicit operator long(FileSize filesize) => filesize.Bytes;
    }

}
