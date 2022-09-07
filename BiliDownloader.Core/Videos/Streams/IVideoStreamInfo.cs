using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public interface IVideoStreamInfo : IStreamInfo
    {
        /// <summary>
        /// 视频编码器
        /// </summary>
        string VideoCodec { get; }

        /// <summary>
        /// 视频的分辨率
        /// </summary>
        int VideoQuality { get; }
        
        int BandWidth { get; }
    }
}
