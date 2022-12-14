using System.IO;

namespace BiliDownloader.Utils
{
    internal static class FileNameGenerator
    {
        private static readonly char[] _trimChars = new[] { ' ', '.' }; 
        public static string GetFullFileName(string dir, string videoName, string videoPartName, string format)
        {
            var videoNameTmp = PathEx.EscapeFileName(videoName.Trim(_trimChars));
            var videoPartNameTmp = PathEx.EscapeFileName(videoPartName.Trim(_trimChars));

            var filePath = Path.Combine(dir, videoNameTmp, videoPartNameTmp);
            return filePath + $".{format}";
        }
    }
}
