using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public class ClosedCaptionTrackInfo
    {
        private static readonly Uri _uriBase = new("https://i0.hdslb.com");
        public Uri Url { get;  }
        public Language Language { get; }

        public ClosedCaptionTrackInfo(string url,Language language)
        {
            Url = new Uri(_uriBase, url);
            Language = language;
        }
    }

}
