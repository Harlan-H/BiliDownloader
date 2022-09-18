using BiliDownloader.Core.ClosedCaptions;
using BiliDownloader.Core.Common;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;

namespace BiliDownloader.Core.Videos
{
    public interface IVideo
    {
        string Title { get; }
        string Description { get; }
        Author? Author { get; }
        TimeSpan? Duration { get; }
        string? Thumbnail { get; }
        IList<IPlaylist> PlayLists { get; }
    }
}
