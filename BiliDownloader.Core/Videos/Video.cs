using BiliDownloader.Core.ClosedCaptions;
using BiliDownloader.Core.Common;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;

namespace BiliDownloader.Core.Videos
{
    public class Video(
        string title,
        string description,
        Author? author,
        TimeSpan? duration,
        string? thumbnail,
        IList<IPlaylist> playLists
            ) : IVideo
    {
        public string Title { get; } = title;
        public string Description { get; } = description;
        public Author? Author { get; } = author;
        public TimeSpan? Duration { get; } = duration;
        public string? Thumbnail { get; } = thumbnail;
        public IList<IPlaylist> PlayLists { get; } = playLists;
    }
}
