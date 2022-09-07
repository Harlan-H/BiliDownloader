using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class VideoOnlyStreamInfo : IVideoStreamInfo
    {
        public string VideoCodec { get; }

        public int VideoQuality { get; }

        public string Url { get;} 


        public FileSize FileSize { get; private set; }

        public int BandWidth { get; }
        public VideoOnlyStreamInfo(
            string url,
            int videoQuality,
            string videoCodec,
            FileSize filesize,
            int bandWidth)
        {
            Url = url;
            VideoCodec = videoCodec;
            FileSize = filesize;
            VideoQuality = videoQuality;
            BandWidth = bandWidth;
        }

        public void UpdateFileSize(FileSize fileSize)
        {
            FileSize = fileSize;
        }
    }
}
