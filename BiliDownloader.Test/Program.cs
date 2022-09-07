using BiliDownloader.Core;
using BiliDownloader.Core.Videos.Streams;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace BiliDownloader.Test
{
    class Program
    {

        //         private static X509Certificate2 GetMyCert()
        //         {
        //             string certThumbprint = "1a4aa22f0f780fc2e1bf19cf4b8b4e4ba933217e";
        //             X509Certificate2 cert = null;
        // 
        //             // Load the certificate
        //             var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        //             store.Open(OpenFlags.ReadOnly);
        //             X509Certificate2Collection certCollection = store.Certificates.Find
        //             (
        //                 X509FindType.FindByThumbprint,
        //                 certThumbprint,
        //                 false    // Including invalid certificates
        //             );
        //             if (certCollection.Count > 0)
        //             {
        //                 cert = certCollection[0];
        //             }
        //             store.Close();
        // 
        //             return cert;
        //         }


        //         private static bool ValidateRemoteCertificate(
        //               object sender,
        //               X509Certificate certificate,
        //               X509Chain chain,
        //               SslPolicyErrors policyErrors)
        //         {
        //             // Logic to determine the validity of the certificate
        //             // return boolean
        //             return true;
        //         }

        /*
Testing SSL server upos-sz-mirrorbsct.bilivideo.com on port 443 using SNI name upos-sz-mirrorbsct.bilivideo.com

  SSL/TLS Protocols:
SSLv2     disabled
SSLv3     disabled
TLSv1.0   disabled
TLSv1.1   disabled
TLSv1.2   disabled
TLSv1.3   enabled       只支持1.3<==

  TLS Fallback SCSV:
Server supports TLS Fallback SCSV

  TLS renegotiation:
Session renegotiation not supported

  TLS Compression:
Compression disabled

  Heartbleed:
TLSv1.3 not vulnerable to heartbleed

  Supported Server Cipher(s):
Preferred TLSv1.3  128 bits  TLS_AES_128_GCM_SHA256        Curve 25519 DHE 253
Accepted  TLSv1.3  256 bits  TLS_AES_256_GCM_SHA384        Curve 25519 DHE 253
Accepted  TLSv1.3  256 bits  TLS_CHACHA20_POLY1305_SHA256  Curve 25519 DHE 253

  Server Key Exchange Group(s):
TLSv1.3  128 bits  secp256r1 (NIST P-256)
TLSv1.3  192 bits  secp384r1 (NIST P-384)
TLSv1.3  260 bits  secp521r1 (NIST P-521)
TLSv1.3  128 bits  x25519
TLSv1.3  224 bits  x448

  SSL Certificate:
Signature Algorithm: sha256WithRSAEncryption
RSA Key Strength:    2048

Subject:  *.bilivideo.com
Altnames: DNS:*.bilivideo.com, DNS:bilivideo.com
Issuer:   GlobalSign RSA OV SSL CA 2018         
         */
        static async Task Main(string[] args)
        {
            //             ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(
            //                ValidateRemoteCertificate
            //            );
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13| SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls ;
            //             const string DisableCachingName = @"TestSwitch.LocalAppContext.DisableCaching";
            //             const string DontEnableSchUseStrongCryptoName = @"Switch.System.Net.DontEnableSchUseStrongCrypto";
            //             AppContext.SetSwitch(DisableCachingName, true);
            //             AppContext.SetSwitch(DontEnableSchUseStrongCryptoName, true);
            var handler = new HttpClientHandler
            {
                UseCookies = false,
            };

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            //             X509Certificate2 cert = GetMyCert();
            //             if (cert != null)
            //             {
            //                 handler.ClientCertificates.Add(cert);
            //             }

            handler.SslProtocols = /* SslProtocols.Tls13 |*/ SslProtocols.Tls12 | SslProtocols.None;
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient httpClient = new(handler);

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36");
            //  httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

            BiliDownloaderClient biliDownloaderClient = new(httpClient);
            CancellationTokenSource cancellationTokenSource = new();

            //FileInfo fileInfo = new(@"E:\desktop\bilibili\直播数据\23385414.html");
           // var data = await biliDownloaderClient.Lives.GetLiveInfoAsync(fileInfo, "https://live.bilibili.com/23385414");

            var data = await biliDownloaderClient.Videos.GetVideoInfoAsync("https://www.bilibili.com/video/BV18r4y1878F?p=8", cancellationTokenSource.Token);
            //var playlist = data.PlayLists[18];
            var playlist = data.PlayLists[17];

            var streamManifest = await biliDownloaderClient.Videos.Streams.GetStreamManifestAsync(playlist, cancellationTokenSource.Token);
            var streamInputs = new List<StreamInput>();
            string filePath = @"E:\desktop\download\BV1jS4y1e7rj";
            try
            {
                await biliDownloaderClient.Videos.Streams.GetVideoDownloadOptionsAsync(streamManifest, filePath, streamInputs, cancellationTokenSource.Token);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            foreach (var streamInput in streamInputs)
            {
                await biliDownloaderClient.Videos.Streams.DownloadAsync(streamInput.Info, streamInput.FilePath);
                Console.WriteLine($"文件 => {streamInput.FilePath}  大小 => {streamInput.Info.FileSize}");
                //streamInput.Dispose();
            }
            Console.WriteLine("完成");
            Console.ReadLine();
        }

    }
}
