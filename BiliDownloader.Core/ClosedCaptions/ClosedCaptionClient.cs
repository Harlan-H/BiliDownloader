using BiliDownloader.Core.Extractors;
using BiliDownloader.Core.Utils.Extensions;
using BiliDownloader.Core.Videos.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public class ClosedCaptionClient
    {
        private readonly CloseCaptionController controller;


        public ClosedCaptionClient(HttpClient httpClient)
        {
            controller = new CloseCaptionController(httpClient);
        }

        public async ValueTask<ClosedCaptionManifest> GetClosedCaptionManifestAsync(
            IPlaylist playlist,
            CancellationToken cancellationToken = default)
        {
            var closeCaptionResponse = await controller.GetClosedCaptionResponseAsync(playlist, cancellationToken);
            return GetClosedCaptionManifest(closeCaptionResponse.TryCloseCaptionTrace());
        }


        public static ClosedCaptionManifest GetClosedCaptionManifest(IReadOnlyList<ClosedCaptionTraceInfoExtractor> closedCaptionTrackInfoExtrctors)
        {
            
            List<ClosedCaptionTrackInfo> closedCaptionTrackInfos = new(closedCaptionTrackInfoExtrctors.Count);
            foreach (var closedCaptionTrackInfoExtractor in closedCaptionTrackInfoExtrctors)
            {
                string url = closedCaptionTrackInfoExtractor.TryGetUrl()
                                ?? throw new InvalidDataException("无法获得字幕地址");

                string code = closedCaptionTrackInfoExtractor.TryGetLanguageCode()
                                ?? throw new InvalidDataException("无法获得字幕文本");

                string name = closedCaptionTrackInfoExtractor.TryGetLanguageName()
                                ?? throw new InvalidDataException("无法获得字幕文本");

                Language language = new(code, name);
                closedCaptionTrackInfos.Add(new ClosedCaptionTrackInfo(url, language));
            }
            return new ClosedCaptionManifest(closedCaptionTrackInfos);
        }


        public void PopulateSubtitleInputsAsync(ClosedCaptionManifest closedCaptionManifest,string filePath, ICollection<ClosedCaptionInput> closedCaptionInputs)
        {
            foreach (var item in closedCaptionManifest.TrackInfos)
            {
                string subtitleName = Path.ChangeExtension(filePath, $"{item.Language.Code}.srt");
                ClosedCaptionInput closedCaptionInput = new(item, subtitleName);
                closedCaptionInputs.Add(closedCaptionInput);
            }
        }

        public async ValueTask<ClosedCaptionTrack> GetClosedCaptionTrack(
            ClosedCaptionTrackInfo trackInfo,
            CancellationToken cancellationToken = default)
        {
            var trackExtract = await controller.GetClosedCaptions(trackInfo, cancellationToken);

            var closedcaptions = trackExtract
                .TryClosedCaptionExtractor()
                .Select(i =>
                {
                    var text = i.TryGetText();
                    if (string.IsNullOrWhiteSpace(text))
                        return null;

                    if (i.TryGetFrom() is not { } from)
                        return null;

                    if (i.TryGetTo() is not { } to)
                        return null;

                    return new ClosedCaption(text, from, to);
                })
                .WhereNotNull()
                .ToArray();

            return new ClosedCaptionTrack(closedcaptions);
        }


        public async ValueTask WriteToAsync(
            ClosedCaptionTrackInfo trackInfo,
            TextWriter writer,
            IProgress<double>? updateCallback = null,
            CancellationToken cancellationToken = default)
        {
            var closedCaptionTrack = await GetClosedCaptionTrack(trackInfo, cancellationToken);

            var buffer = new StringBuilder();
            for (int i = 0; i < closedCaptionTrack.Captions.Count; i++)
            {
                var caption = closedCaptionTrack.Captions[i];
                buffer.Clear();

                cancellationToken.ThrowIfCancellationRequested();

                buffer.AppendLine((i + 1).ToString());

                buffer.Append(caption.From.ToString(@"hh\:mm\:ss\,fff"));
                buffer.Append(" --> ");
                buffer.Append(caption.To.ToString(@"hh\:mm\:ss\,fff"));
                buffer.AppendLine();

                buffer.AppendLine(caption.Content);

                await writer.WriteLineAsync(buffer, cancellationToken);
            }
        }

        public async ValueTask DownloadAsync(
            ClosedCaptionTrackInfo trackInfo,
            string filePath,
            IProgress<double>? updateCallback = null,
            CancellationToken cancellationToken = default)
        {
            using var writer = File.CreateText(filePath);
            await WriteToAsync(trackInfo, writer, updateCallback, cancellationToken);
        }
    }
}
