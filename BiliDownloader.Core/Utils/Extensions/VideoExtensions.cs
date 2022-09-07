using BiliDownloader.Core.Videos;
using BiliDownloader.Core.Videos.Pages;
using System.Collections.Generic;

namespace BiliDownloader.Core.Utils.Extensions
{
    public static class VideoExtensions
    {
        public static bool IsVideoCollection(this IVideo video, VideoId videoId)
        {
            bool IsCollection = false;
            if (video.PlayLists.Count < 2)
                return IsCollection;

            for (int i = 0; i < 2; i++)
            {
                PlayList playList = (PlayList)video.PlayLists[i];
                //合集的valueid是不同的 所以只要判断到不同的 就说明是合集
                if (videoId.Value != playList.Id.Value)
                {
                    IsCollection = true;
                    break;
                }
            }
            return IsCollection;
        }

        public static IVideo GetSingleVideoPlayListFromVideoCollection(this IVideo video, VideoId videoId)
        {
            foreach (PlayList item in video.PlayLists)
            {
                if (item.Id.Value == videoId.Value)
                {
                    return new Video(video.Title, video.Description, video.Author, video.Duration, video.Thumbnail, new List<IPlaylist> { item });
                }
            }
            return video;
        }
    }
}
