using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.ViewModels.Dialogs
{
    public interface IViewModelFactory
    {
        DownloadViewModel CreateDownloadViewModel();

        DownloadMultipleSetupViewModel CreateDownloadMultipleSetupViewModel();

        MessageBoxViewModel CreateMessageBoxViewModel();

        SettingsViewModel CreateSettingsViewModel();
    }
}
