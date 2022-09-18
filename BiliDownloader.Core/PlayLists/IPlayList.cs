using BiliDownloader.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Pages
{
    public interface IPlaylist
    {
        string Url { get; }
        string ClosedCaptionUrl { get; }
        string? Title { get; }
        TimeSpan? Duration { get; }
    }
}
