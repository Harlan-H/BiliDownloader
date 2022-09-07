using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public interface IStreamInfo
    {
        string Url { get; }
        FileSize FileSize { get; }
    }
}
