using BiliDownloader.Services;
using BiliDownloader.ViewModels.Dialogs;
using System.Collections.Generic;

namespace BiliDownloader.ViewModels
{
    public class SettingsViewModel(SettingsService settingsService) : DialogScreen
    {
        private readonly SettingsService settingsService = settingsService;

        public SettingsService SettingsServiceClone { get; set; } = (SettingsService)settingsService.Clone();

        public IReadOnlyList<string> AvailableFormats { get; set; } = ["flv","mp4"];


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
