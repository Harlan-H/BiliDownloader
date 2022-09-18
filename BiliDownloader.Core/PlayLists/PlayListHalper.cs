using BiliDownloader.Core.Extractors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiliDownloader.Core.Videos.Pages
{
    internal static class PlayListHelper
    {

        /// <summary>
        /// 获取合集
        /// </summary>
        /// <param name="videoPageExtractor">主页数据</param>
        /// <returns>视频流信息</returns>
        public static IList<IPlaylist> GetPlayLists(VideoPageExtractor videoPageExtractor)
        {
            var Episodes = videoPageExtractor.TryGetEpisodes();
            if (!Episodes.Any())
                return null!;

            IList<IPlaylist> pages = new List<IPlaylist>();
            foreach (var e in Episodes)
            {
                var id = e.TryGetBvid()!;
                var aid = e.TryGetAid() ?? 0;
                var cid = e.TryGetCid() ?? 0;
                var title = e.TryGetTitle();
                var duration = e.TryGetDuration() ?? TimeSpan.Zero;

                var playlist = new PlayList(id,aid, cid, title, duration);
                pages.Add(playlist);
            }
            return pages;
        }


        /// <summary>
        /// 获取单集或多集
        /// </summary>
        /// <param name="videoPageExtractor">主页数据</param>
        /// <param name="videoId">bvid</param>
        /// <returns>视频流信息</returns>
        public static IList<IPlaylist> GetPlayLists(VideoPageExtractor videoPageExtractor,int aid, VideoId videoId)
        {
            var pageExtractor = videoPageExtractor.TryGetPages();
            if (!pageExtractor.Any()) return Array.Empty<IPlaylist>();

            IList<IPlaylist> pages = new List<IPlaylist>();
            foreach (var p in pageExtractor)
            {
                var cid = p.TryGetCid() ?? 0;
                var title = p.TryGetTitle();
                var duration = p.TryGetDuration() ?? TimeSpan.FromSeconds(0);
                pages.Add(new PlayList(videoId, aid, cid, title, duration));
            }
            return pages;
        }
    }
}
