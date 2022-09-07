using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class StreamInfo : IStreamInfo
    {
        public string Url { get; }

        public FileSize FileSize { get; }
        public StreamInfo(string url,FileSize fileSize)
        {
            Url = url;
            FileSize = fileSize;
        }
    }
}
