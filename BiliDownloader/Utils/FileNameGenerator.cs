using System.IO;

namespace BiliDownloader.Utils
{
    internal static class FileNameGenerator
    {
        public static string GetFullFileName(string dir, string videoName, string videoPartName, string format)
        {
            var videoNameTmp = PathEx.EscapeFileName(videoName.Trim());
            var videoPartNameTmp = PathEx.EscapeFileName(videoPartName.Trim());

            var filePath = Path.Combine(dir, videoNameTmp, videoPartNameTmp);
            return filePath + $".{format}";
        }
    }
}
