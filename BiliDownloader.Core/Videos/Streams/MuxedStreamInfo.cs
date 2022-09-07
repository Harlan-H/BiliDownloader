using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class MuxedStreamInfo : IStreamInfo
    {
        public string Url { get; }
        public FileSize FileSize { get; }

        public MuxedStreamInfo(string url,FileSize fileSize)
        {
            Url = url;
            FileSize = fileSize;
        }
    }
}
