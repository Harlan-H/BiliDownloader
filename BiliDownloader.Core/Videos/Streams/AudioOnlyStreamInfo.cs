using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class AudioOnlyStreamInfo(
        string url,
        int audioquality,
        string audiocodec,
        FileSize fileSize,
        int bandWidth) : IAudioStreamInfo
    {
        public string AudioCodec { get; } = audiocodec;

        public string Url { get; } = url;

        public FileSize FileSize { get; private set; } = fileSize;

        public int BandWidth { get; } = bandWidth;

        public int AudioQuality { get; } = audioquality;

        public void UpdateFileSize(FileSize fileSize)
        {
            FileSize = fileSize;
        }
    }
}
