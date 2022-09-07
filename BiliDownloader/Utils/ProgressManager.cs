using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Utils
{
    public class ProgressManager : IProgress<long>, IDisposable
    {
        public long Progress { get; private set; }

        public bool IsSuccessed { get; private set; }

        public void Dispose() => IsSuccessed = true;

        public void Report(long value) => Progress = value;

    }
}
