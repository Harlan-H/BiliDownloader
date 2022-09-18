using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public class ClosedCaptionInput : IDisposable
    {
        public ClosedCaptionTrackInfo TrackInfo { get; }
        public string FilePath { get; }

        public ClosedCaptionInput(ClosedCaptionTrackInfo trackInfo, string filePath)
        {
            TrackInfo = trackInfo;
            FilePath = filePath;
        }

        public void Dispose()
        {
            try
            {
                File.Delete(FilePath);
                GC.SuppressFinalize(this);
            }catch
            {

            }
        }
    }
}
