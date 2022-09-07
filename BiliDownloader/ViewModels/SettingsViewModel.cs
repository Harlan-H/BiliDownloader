using BiliDownloader.Services;
using BiliDownloader.ViewModels.Dialogs;
using System.Collections.Generic;

namespace BiliDownloader.ViewModels
{
    public class SettingsViewModel : DialogScreen
    {
        private readonly SettingsService settingsService;

        public SettingsViewModel(SettingsService settingsService)
        {
            this.settingsService = settingsService;
            SettingsServiceClone = (SettingsService)settingsService.Clone();
        }

        public SettingsService SettingsServiceClone { get; set; }

        public IReadOnlyList<string> AvailableFormats { get; set; } = new[] { "flv","mp4" };


        public void OnCloseDialog()
        {
            Close(false);
        }

        public void OnSubmitSettingInfo(SettingsService obj)
        {
            obj.ServiceUpdate(settingsService);
            settingsService.CopyFrom(obj);
            Close(true);
        }

    }
}
