using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public class ClosedCaptionManifest
    {
        public IReadOnlyList<ClosedCaptionTrackInfo> TrackInfos { get; }

        public ClosedCaptionManifest(IReadOnlyList<ClosedCaptionTrackInfo> trackInfos)
        {
            TrackInfos = trackInfos;
        }

        public ClosedCaptionTrackInfo? TryGetLanguage(string language) =>
            TrackInfos.FirstOrDefault(t => string.Equals(t.Language.Code, language, StringComparison.OrdinalIgnoreCase));

        public ClosedCaptionTrackInfo GetLanguage(string language) =>
            TryGetLanguage(language) ??
            throw new InvalidOperationException($"没有指定得字幕 {language}");


        public ClosedCaptionTrackInfo? TryGetLanguage(ClosedCaptionTrackInfo closedCaptionTrackInfo) =>
            TryGetLanguage(closedCaptionTrackInfo.Language.Code);

        public ClosedCaptionTrackInfo GetLanguage(ClosedCaptionTrackInfo closedCaptionTrackInfo) =>
            TryGetLanguage(closedCaptionTrackInfo) ??
            throw new InvalidOperationException($"没有指定得字幕 {closedCaptionTrackInfo.Language}");


        }
}
