using System;
using System.Net;
using System.Net.Http;

namespace BiliDownloader.Utils
{
    public class Http
    {
        private static HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = false,
            };

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

//             handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
//             handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13 | SslProtocols.None;
            return handler;
        }

        public static HttpClient Client { get; }

        static Http()
        {
            Client = new(GetHandler());
            Client.Timeout = TimeSpan.FromSeconds(10);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
           // Client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        }

        public static void AddCookie(string cookie)
        {
            if (!string.IsNullOrWhiteSpace(cookie))
                Client.DefaultRequestHeaders.Add("cookie", cookie);
            else
                Client.DefaultRequestHeaders.Remove("cookie");
        }
    }
}
