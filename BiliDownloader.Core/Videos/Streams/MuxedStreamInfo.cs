using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class MuxedStreamInfo(string url, FileSize fileSize) : IStreamInfo
    {
        public string Url { get; } = url;
        public FileSize FileSize { get; } = fileSize;
    }
}
