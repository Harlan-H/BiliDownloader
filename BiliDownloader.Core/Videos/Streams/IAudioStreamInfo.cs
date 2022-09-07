using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public interface IAudioStreamInfo : IStreamInfo
    {
        /// <summary>
        /// 音频编码器
        /// </summary>
        string AudioCodec { get; }

        /// <summary>
        /// 音频的质量就是码率 
        /// 可能是30280 30232  30216  
        /// 代表3中不同的码率 302 可能是固定的
        /// </summary>
        int AudioQuality { get; }

        int BandWidth { get; }
    }
}
