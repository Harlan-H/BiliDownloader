using BiliDownloader.Core.Videos.Pages;
using BiliDownloader.Core.Videos.Streams;
using BiliDownloader.Models;
using BiliDownloader.Services;
using BiliDownloader.Utils;
using BiliDownloader.ViewModels.Dialogs;
using PropertyChanged;
using Stylet;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.ViewModels
{
    public class DownloadViewModel : PropertyChangedBase, IDisposable
    {
        private CancellationTokenSource? cancellationTokenSource;
        private readonly DownloadService downloadService;
        private readonly SoundsService soundsService;

        [DoNotNotify]
        public IPlaylist Playlist { get; set; } = default!;

        [DoNotNotify]
        public string FilePath { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string CurrentSpeed { get; private set; } = default!;
        public TimeSpan Duration { get; private set; }
        public double ProgressNum { get; private set; }
        public FileSize Filesize { get; private set; }

        public bool IsActive { get; private set; }
        public DownloadStatus Status { get; set; }

        public bool IsDownloading => Status == DownloadStatus.Downloaded;
        public string? FailReason { get; private set; } = string.Empty;

        public DownloadViewModel(DownloadService downloadService,SoundsService soundsService)
        {
            this.downloadService = downloadService;
            this.soundsService = soundsService;
        }

        private async void RefreshSpeedAndTime(FileSize filesize, ProgressManager progressManager)
        {
            Filesize = filesize;
            Status = DownloadStatus.Downloaded;

            Speed speed = new();
            DurationTime durationTime = new(filesize);
            while (!progressManager.IsSuccessed)
            {
                CurrentSpeed = speed.GetNext(progressManager.Progress);
                Duration = durationTime.GetNext(progressManager.Progress);

                if (filesize > 0)
                    ProgressNum = progressManager.Progress / (double)filesize;

                await Task.Delay(1000);
            }

        }

        public bool CanOnStart => !IsActive;
        public void OnStart()
        {
            if (!CanOnStart)
                return;

            IsActive = true;

            _ = Task.Run(async () =>
              {
                  cancellationTokenSource = new CancellationTokenSource();

                  try
                  {
                      //                       streamInfos ??= await downloadService.GetDownloadOptions(Playlist, cancellationTokenSource.Token);
                      // 
                      //                       var streaminfo = streamInfos.First();
                      //                       FileLength = streaminfo.FileSize;
                      // 
                      //                       _ = Task.Run(UpdateSpeedAndTime);
                      // 
                      //                       IsDownloading = true;
                      //                       await downloadService.DownloadAsync(streaminfo, FilePath, UpdatePositionCallBack, cancellationTokenSource.Token);

                      Status = DownloadStatus.Enqueued;
                      await downloadService.DownloadAsync(Playlist, FilePath, RefreshSpeedAndTime, cancellationTokenSource.Token);

                      soundsService.PlaySuccess();
                      Status = DownloadStatus.Completed;
                  }
                  catch (OperationCanceledException) when(cancellationTokenSource.IsCancellationRequested)
                  {
                      Status = DownloadStatus.Canceled;
                  }
                  catch (Exception e)
                  {
                      soundsService.PlayError();
                      Status = DownloadStatus.Failed;
#if DEBUG
                      FailReason = e.ToString();
#else
                      FailReason = e.Message;
#endif
                  }
                  finally
                  {
                      IsActive = false;
                      cancellationTokenSource?.Dispose();
                  }
              });
        }


        public bool CanOnCancel => IsActive && Status != DownloadStatus.Canceled;  //!IsCanceled;
        public void OnCancel()
        {
            if (!CanOnCancel)
                return;

            cancellationTokenSource?.Cancel();
        }

        public bool CanOnRestart => CanOnStart && Status != DownloadStatus.Completed;// !IsSuccessful;
        public void OnRestart() => OnStart();

        public bool CanOnShowFile => Status == DownloadStatus.Completed;
        public void OnShowFile()
        {
            if (!CanOnShowFile)
                return;

            try
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                    return;

                Process.Start("explorer", $"/select, \"{FilePath}\"");
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public static class DownloadViewModelExtensions
    {
        public static DownloadViewModel CreateDownloadViewModel(this IViewModelFactory viewModelFactory, IPlaylist playlist,string filePath)
        {
            var view = viewModelFactory.CreateDownloadViewModel();
            view.Playlist = playlist;
            view.Title = playlist.Title!;
            view.FilePath = filePath;

            return view;
        }
    }
}
