using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Controllers
{
    internal abstract class BaseController
    {
        private readonly HttpClient httpClient;

        public BaseController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        protected async ValueTask<string> SendHttpRequestAsync(
            HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken = default)
        {
//             if(!httpRequestMessage.Headers.Contains("User-Agent"))
//             {
//                 httpRequestMessage.Headers.Add("User-Agent",
//                     "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36");
//             }

            using var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"请求失败 错误码是:{response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        protected async ValueTask<string> SendHttpRequestAsync(
            string url,
            CancellationToken cancellationToken = default
            )
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await SendHttpRequestAsync(request, cancellationToken);
        }
    }
}
