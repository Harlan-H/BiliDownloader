using BiliDownloader.Core.Extractors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiliDownloader.Core.Videos.Pages
{
    internal static class PlayListHelper
    {
        private static PlayList GetPage(PagesExtractor pageExtractor, VideoId videoId)
        {
            var cid = pageExtractor.TryGetCid() ?? 0;
            var title = pageExtractor.TryGetTitle();
            var duration = pageExtractor.TryGetDuration() ?? TimeSpan.FromSeconds(0);
            return new PlayList(videoId, cid, title, duration);
        }

        public static IList<IPlaylist> GetPlayLists(VideoPageExtractor videoPageExtractor)
        {
            var Episodes = videoPageExtractor.TryGetEpisodes();
            if (!Episodes.Any())
                return null!;

            IList<IPlaylist> pages = new List<IPlaylist>();
            foreach (var e in Episodes)
            {
                var id = e.TryGetBvid()!;
                var cid = e.TryGetCid() ?? 0;
                var title = e.TryGetTitle();
                var duration = e.TryGetDuration() ?? TimeSpan.Zero;

                var playlist = new PlayList(id, cid, title, duration);
                pages.Add(playlist);
            }
            return pages;
        }

        public static IList<IPlaylist> GetPlayLists(VideoPageExtractor videoPageExtractor, VideoId videoId)
        {
            var pageExtractor = videoPageExtractor.TryGetPages();
            if (!pageExtractor.Any()) return Array.Empty<IPlaylist>();

            IList<IPlaylist> pages = new List<IPlaylist>();
            foreach (var p in pageExtractor)
            {
                var pagetmp = GetPage(p, videoId);
                pages.Add(pagetmp);
            }
            return pages;
        }
    }
}
