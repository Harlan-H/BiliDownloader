using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class AudioOnlyStreamInfo : IAudioStreamInfo
    {
        public string AudioCodec { get; }

        public string Url { get; } 

        public FileSize FileSize { get; private set; }

        public int BandWidth { get; }

        public int AudioQuality { get; }

        public AudioOnlyStreamInfo(
            string url,
            int audioquality,
            string audiocodec,
            FileSize fileSize,
            int bandWidth)
        {
            Url = url;
            AudioQuality = audioquality;
            AudioCodec = audiocodec;
            FileSize = fileSize;
            BandWidth = bandWidth;
        }

        public void UpdateFileSize(FileSize fileSize)
        {
            FileSize = fileSize;
        }
    }
}
