using BiliDownloader.Core.Common;
using BiliDownloader.Core.Videos;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;

namespace BiliDownloader.Core.Lives
{
    public class LiveInfo : IVideo
    {
        public string Title { get; }

        public string Description { get; }

        public Author? Author { get; }

        public TimeSpan? Duration { get; }

        public string? Thumbnail { get; }

        public IList<IPlaylist> PlayLists { get; }

        public LiveInfo(string title, string desc, Author? author, TimeSpan? timeSpan, string? thumb, IList<IPlaylist> playlists)
        {
            Title = title;
            Description = desc;
            Author = author;
            Duration = timeSpan;
            Thumbnail = thumb;
            PlayLists = playlists;
        }

        public LiveInfo() : this("没有直播", "没有直播", null, TimeSpan.Zero, null, Array.Empty<IPlaylist>())
        {

        }
    }
}
