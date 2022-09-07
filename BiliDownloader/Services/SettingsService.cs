using BiliDownloader.Utils;
using BiliDownloader.Utils.Extensions;
using Settings;
using System.IO;

namespace BiliDownloader.Services
{
    public class SettingsService : SettingsManager
    {
        public int MaxConcurrentDownloadCount { get; set; } = 3;
#if DEBUG
        public string SavePath { get; set; } = @"E:\desktop\download";
#else
        public string SavePath { get; set; } = Path.Combine(System.Environment.CurrentDirectory, "download");
#endif
        public bool ShouldSkipExistingFiles { get; set; } = true;

     //   public string SelectedFormat { get; set; } = "mp4";

        public string Cookies { get; set; } = string.Empty;

        public void ServiceUpdate(SettingsService oldSetting)
        {
            MaxConcurrentDownloadCount = MaxConcurrentDownloadCount.Range(1, 10);

            if (Cookies != oldSetting.Cookies)
                Http.AddCookie(Cookies);
        }

        public void ServiceUpdate()
        {
            ServiceUpdate(new SettingsService());
        }
    }
}
