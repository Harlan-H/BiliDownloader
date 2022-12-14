using BiliDownloader.Core.Videos.Streams;
using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Threading;

namespace BiliDownloader.Utils
{
    public class DownloadRate : PropertyChangedBase, IProgress<long>,IDisposable
    {
        private readonly Timer _timer;
        private long _progress;
        private long _lastBytes;

        public FileSize FileSize { get; private set; }
        public long CurrentRate { get; private set; }
        public TimeSpan Duration { get; private set; }
        public double ProgressNum { get; private set; }

        public DownloadRate(long fileSize)
        {
            FileSize = new FileSize( fileSize);
            _timer = new Timer(s => TimerCallback());
        }

        public void Run()
        {
            _timer.Change(0, 1000);
        }


        private void TimerCallback()
        {
            var bytes = _progress;
            CurrentRate = bytes - _lastBytes;

            Duration = GetDuration(bytes,CurrentRate);

            ProgressNum = (double)_progress / FileSize;

            _lastBytes = bytes;
        }

        private TimeSpan GetDuration(long progressNum,  long currentRate)
        {
            if (currentRate == 0)
                return TimeSpan.Zero;

            return TimeSpan.FromSeconds((FileSize.Bytes - progressNum) / currentRate);
        }

        public void Report(long value) => _progress = value;

        public void Dispose()
        {
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
