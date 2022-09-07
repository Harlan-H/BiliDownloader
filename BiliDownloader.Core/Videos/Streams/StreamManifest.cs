using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Videos.Streams
{
    public class StreamManifest
    {
        private readonly IReadOnlyList<IStreamInfo> streams;

        public StreamManifest(IReadOnlyList<IStreamInfo> streams)
        {
            this.streams = streams;
        }

        public IEnumerable<IAudioStreamInfo> GetAudioStreams() =>
            streams.OfType<IAudioStreamInfo>();

        public IEnumerable<IVideoStreamInfo> GetVideoStreams() =>
            streams.OfType<IVideoStreamInfo>();

        public IEnumerable<MuxedStreamInfo> GetMuxedStreams() =>
            streams.OfType<MuxedStreamInfo>();

        public IEnumerable<AudioOnlyStreamInfo> GetAudioOnlyStreams() =>
            GetAudioStreams().OfType<AudioOnlyStreamInfo>();

        public IEnumerable<VideoOnlyStreamInfo> GetVideoOnlyStreams() =>
            GetVideoStreams().OfType<VideoOnlyStreamInfo>();
    }
}
