using System;

namespace BiliDownloader.Services
{
    public partial class Speed
    {
        private long LastBytes;
        public long Bytes { get; private set; }

        public double KiloBytes => Bytes / 1024.0;

        public double MegaBytes => Bytes / 1048576.0;

        public Speed(long bytes) => Bytes = bytes;

        public Speed() : this(0)
        {

        }

        private void Next(long currentBytes)
        {
            Bytes = currentBytes - LastBytes;
            LastBytes = currentBytes;
        }

        private string GetLargestWholeNumberSymbol()
        {
            return Bytes >= 0x100000 ? "MB" : Bytes is >= 0x400 and < 0x100000 ? "KB" : "B";
        }

        private double GetLargestWholeNumberValue()
        {
            return Bytes >= 0x100000 ? MegaBytes : Bytes is >= 0x400 and < 0x100000 ? KiloBytes : Bytes;
        }

        public string GetNext(long currentBytes)
        {
            Next(currentBytes);
            return ToString();
        }

        public override string ToString() => $"{GetLargestWholeNumberValue():0.#} {GetLargestWholeNumberSymbol()}/s";
    }

    public partial class Speed
    {
        public static implicit operator string (Speed speed) => speed.ToString();
    }

}
