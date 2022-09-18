using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public class ClosedCaptionTrack
    {
        public IReadOnlyList<ClosedCaption> Captions { get; }
        public ClosedCaptionTrack(IReadOnlyList<ClosedCaption> captions)
        {
            Captions = captions;
        }
    }
}
