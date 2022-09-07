using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    public static class HttpExtension
    {
        public static async ValueTask<Stream> GetStreamAsync(this HttpClient httpClient,string url, bool ensureSuccess = true, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!request.Headers.Contains("referer"))
            {
                request.Headers.Add("referer", "https://www.bilibili.com");
            }

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if(ensureSuccess) response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }

        public static async ValueTask<HttpResponseMessage> HeadAsync(this HttpClient httpClient,string requestUri,CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Head, requestUri);
            if (!request.Headers.Contains("referer"))
            {
                request.Headers.Add("referer", "https://www.bilibili.com");
            }
            return await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        public static async ValueTask<long?> TryGetContentLengthAsync(this HttpClient httpClient,string requestUri,
                bool ensureSuccess = true, CancellationToken cancellationToken = default)
        {
            using var response = await httpClient.HeadAsync(requestUri, cancellationToken);
            if (ensureSuccess)
                response.EnsureSuccessStatusCode();

            return response.Content.Headers.ContentLength;
        }
    }
}
