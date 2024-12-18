using System;
using System.IO;

namespace BiliDownloader.Core.Videos.Streams
{
    public class StreamInput(IStreamInfo info, string filePath) : IDisposable
    {
        public IStreamInfo Info { get; } = info;

        public string FilePath { get; } = filePath;

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
