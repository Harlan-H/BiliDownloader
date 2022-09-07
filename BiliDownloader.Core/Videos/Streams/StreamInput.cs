using System;
using System.IO;

namespace BiliDownloader.Core.Videos.Streams
{
    public class StreamInput : IDisposable
    {
        public IStreamInfo Info { get; }

        public string FilePath { get; }

        public StreamInput(IStreamInfo info, string filePath)
        {
            Info = info;
            FilePath = filePath;
        }

        public void Dispose()
        {
            try
            {
                File.Delete(FilePath);
                GC.SuppressFinalize(this);
            }catch
            {

            }
        }
    }
}
