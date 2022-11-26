using BiliDownloader.Core.Videos;
using BiliDownloader.Core.Videos.Pages;
using BiliDownloader.Services;
using BiliDownloader.Utils;
using BiliDownloader.ViewModels.Dialogs;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiliDownloader.ViewModels
{
    public partial class DownloadMultipleSetupViewModel : DialogScreen<IReadOnlyList<DownloadViewModel>>
    {
        private readonly SettingsService settingsService;

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public string? Author { get; set; }

        public TimeSpan? Duration { get; set; }

        public string? Thumbnail { get; set; }

        public string VideoFormat { get; set; } = string.Empty;

        public IList<IPlaylist> AvailableVideos { get; set; } = Array.Empty<IPlaylist>();
        public IList<IPlaylist> SelectedVideos { get; set; } = Array.Empty<IPlaylist>();

        public DownloadMultipleSetupViewModel(SettingsService settingsService)
        {
            this.settingsService = settingsService;
        }


        public bool CanConfirm => AvailableVideos.Any() && SelectedVideos.Any();
        public void Confirm()
        {
            if (!SelectedVideos.Any())
            {
                Close(null);
                return;
            }

            var downloads = new List<DownloadViewModel>();
            foreach (var playlist in SelectedVideos)
            {
                var filePath = FileNameGenerator.GetFullFileName(settingsService.SavePath, Title, playlist.Title!, VideoFormat);
                FileInfo fileInfo = new(filePath);
                if(fileInfo.Exists && fileInfo.Length > 0)
                {
                    if (settingsService.ShouldSkipExistingFiles)
                        continue;

                    filePath = PathEx.MakeUniqueFilePath(filePath);
                }

                PathEx.CreateDirectoryForFile(filePath);

                var download = DownloadViewModel.CreateDownloadViewModel(playlist, filePath);
                downloads.Add(download);
            }

            Close(downloads);
        }


    }

    public partial class DownloadMultipleSetupViewModel
    {
        public static DownloadMultipleSetupViewModel CreateDownloadMultipleSetupViewModel(IVideo video)
        {
            var view = IoC.Get<DownloadMultipleSetupViewModel>();
            view.Title = video.Title;
            view.Description = video.Description;
            view.Author = video.Author?.ToString();
            view.Duration = video.Duration;
            view.Thumbnail = video.Thumbnail;
            view.AvailableVideos = video.PlayLists;
            view.VideoFormat = "mp4";
            return view;
        }
    }
}
