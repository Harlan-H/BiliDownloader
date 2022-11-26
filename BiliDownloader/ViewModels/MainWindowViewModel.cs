using BiliDownloader.Core.Utils.Extensions;
using BiliDownloader.Core.Videos;
using BiliDownloader.Models;
using BiliDownloader.Services;
using BiliDownloader.Utils.Extensions;
using BiliDownloader.ViewModels.Dialogs;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BiliDownloader.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly QueryService queryService;
        private readonly SettingsService settingsService;

        public ISnackbarMessageQueue Snackbar { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
        public BindableCollection<DownloadViewModel> Downloads { get; } = new();
        public string? RequestUrl { get; set; }
        public bool IsBusy { get; private set; }

        public MainWindowViewModel(QueryService queryService, SettingsService settingsService)
        {
            this.queryService = queryService;
            this.settingsService = settingsService;
        }


        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            DisplayName = $"b站视频下载器 v{App.VersionString} by:Harlan";

            settingsService.Load();
            settingsService.ServiceUpdate();

            return base.OnInitializeAsync(cancellationToken);
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            settingsService.Save();
            return base.CanCloseAsync(cancellationToken);
        }


        private static async Task<IVideo> FilterVideoPlaylist(QueryModel queryModel)
        {
            if (queryModel.Id is not VideoId videoId)
                return queryModel.Value;

            var video = queryModel.Value;
            if(video.IsVideoCollection(videoId))
            {
                var view = MessageBoxViewModel.CreateMessageBoxViewModel("注意", "当前视频包含合集,是否展示合集所有内容", "是", "否");
                var ret = await DialogManager.ShowDialogAsync(view);
                return ret ? queryModel.Value : video.GetSingleVideoPlayListFromVideoCollection(videoId);
            }
            return queryModel.Value;
        }

        private void EnqueueDownload(DownloadViewModel download)
        {
            var existingDownloads = Downloads.Where(d => d.Title == download.Title).ToArray();
            foreach (var existingDownload in existingDownloads)
            {
                existingDownload.OnCancel();
                Downloads.Remove(existingDownload);
            }

            download.OnStart();
            Downloads.Insert(0, download);
        }

        public bool CanStartDownload => !IsBusy && !string.IsNullOrWhiteSpace(RequestUrl);
        public async void StartDownload()
        {
            IsBusy = true;

            try
            {
                QueryModel query = await queryService.ParseQuery(RequestUrl);
                // await urlService.ParseQuery(new FileInfo(@"E:\desktop\bilibili\网页源码\6.html"), "BV3212624");
                var video = await FilterVideoPlaylist(query);
                var view = DownloadMultipleSetupViewModel.CreateDownloadMultipleSetupViewModel(video);
                view.SelectedVideos = view.AvailableVideos;

                var downloads = await DialogManager.ShowDialogAsync(view);
                if (downloads is null)
                    return;

                foreach (var download in downloads)
                    EnqueueDownload(download);

                RequestUrl = string.Empty;
            }
            catch (Exception e)
            {
                Snackbar.Enqueue(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public void RemoveDownload(DownloadViewModel download)
        {
            download.OnCancel();
            Downloads.Remove(download);
        }

        public async void RemoveAllDownloads()
        {
            var result = await MessageBoxViewModel.ShowMessageBox("注意", "是否要删除所有下载?");
            if (result == false) return;

            foreach (var download in Downloads.ToArray())
            {
                download.OnCancel();
                Downloads.Remove(download);
            }
        }

        public void RemoveInactiveDownloads()
        {
            Downloads.RemoveWhere(c => !c.IsActive);
        }

        public void RemoveSuccessDownloads()
        {
            Downloads.RemoveWhere(c => c.Status == DownloadStatus.Completed);
        }

        public void CancelAllDownloads()
        {
            foreach (var download in Downloads)
            {
                download.OnCancel();
            }
        }

        public void RestartFailedDownloads()
        {
            var failedDownloads = Downloads.Where(c => c.Status == DownloadStatus.Failed);
            foreach (var failedDownload in failedDownloads)
                failedDownload.OnRestart();

        }

        public void CopyTitle(DownloadViewModel download) => Clipboard.SetText(download.Title);


        public async void ShowSettings()
        {
            var dialog = new SettingsViewModel(settingsService);
            await DialogManager.ShowDialogAsync(dialog);
        }
    }
}
