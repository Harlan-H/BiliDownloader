using BiliDownloader.Core.Common;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;

namespace BiliDownloader.Core.Videos
{
    public class Video : IVideo
    {
        public string Title { get; }
        public string Description { get; }
        public Author? Author { get; }
        public TimeSpan? Duration { get; }
        public string? Thumbnail { get; }
        public IList<IPlaylist> PlayLists { get; }
        public Video(
            string title,
            string description,
            Author? author,
            TimeSpan? duration,
            string? thumbnail,
            IList<IPlaylist> playLists
            )
        {
            Title = title;
            Description = description;
            Author = author;
            Duration = duration;
            Thumbnail = thumbnail;
            PlayLists = playLists;
        }
    }
}
