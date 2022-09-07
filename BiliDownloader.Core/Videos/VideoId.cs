using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Text.RegularExpressions;


namespace BiliDownloader.Core.Videos
{
    public readonly partial struct VideoId
    {
        public string Value { get; }

        public string Url => $"https://www.bilibili.com/video/{Value}";

        private VideoId(string value) => Value = value;

        public override string ToString() => Value;
    }

    public readonly partial struct VideoId
    {
        private static bool IsValid(string videoId)
        {
            if (!videoId.StartsWith("BV"))
                return false;

            return !Regex.IsMatch(videoId, @"[^0-9a-zA-Z]");
        }

        private static string? TryNormalize(string? videoIdOrUrl)
        {
            if (string.IsNullOrWhiteSpace(videoIdOrUrl))
                return null;

            if (IsValid(videoIdOrUrl))
                return videoIdOrUrl;

            //匹配 BV1o94y1Z75J
            //https://www.bilibili.com/video/BV1o94y1Z75J?spm_id_from=333.1007.partition_recommend.content.click
            //https://www.bilibili.com/video/BV1HL4y1M7Es?share_source=copy_web
            var regularMatch = Regex.Match(videoIdOrUrl, @"bilibili\..+?video/(.*?)(?:\?|$|/\?)").Groups[1].Value;
            if (!string.IsNullOrWhiteSpace(regularMatch) && IsValid(regularMatch))
                return regularMatch;

            return null;
        }

        public static VideoId? TryParse(string? videoIdOrUrl) => TryNormalize(videoIdOrUrl)?.Pipe(p => new VideoId(p));

        public static VideoId Parse(string videoIdOrUrl) => TryParse(videoIdOrUrl) ??
            throw new ArgumentException($"无效的视频id或者url {videoIdOrUrl}");

        public static implicit operator VideoId(string videoIdOrUrl) => Parse(videoIdOrUrl);

        public static implicit operator string(VideoId videoId) => videoId.ToString();
    }

    public partial struct VideoId : IEquatable<VideoId>
    {
        public bool Equals(VideoId other) => StringComparer.Ordinal.Equals(Value, other.Value);
        public override bool Equals(object? obj) => obj is VideoId other && Equals(other);
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

        public static bool operator == (VideoId videoId, VideoId videoId1) => videoId.Equals(videoId1);
        public static bool operator !=(VideoId videoId, VideoId videoId1) => !(videoId == videoId1);
    }
}
