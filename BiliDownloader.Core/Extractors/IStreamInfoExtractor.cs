using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Extractors
{
    internal interface IStreamInfoExtractor
    {
        bool IsVideoStream { get; }
        string? TryGetDashType();
        string? TryGetUrl();
        int? TryGetFileSize();
        int? TryGetBandWidth();
        string? TryGetCodec();
        int? TryGetQuality();
    }
}
