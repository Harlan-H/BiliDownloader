using BiliDownloader.Services;
using BiliDownloader.ViewModels;
using BiliDownloader.ViewModels.Dialogs;
using Stylet;
using StyletIoC;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;

namespace BiliDownloader
{
    public class Bootstrapper : Bootstrapper<MainWindowViewModel>
    {

        protected override void OnStart()
        {
            base.OnStart();

            ServicePointManager.DefaultConnectionLimit = 40;
            HttpClient.DefaultProxy = new WebProxy();

            //The ssl connection could not be established,see inner exception.
            //https://docs.microsoft.com/en-us/answers/questions/400152/authentication-failed-because-the-remote-party-has.html
            //主要的原因是 尝试调用外部 API 时可能会出现此错误。此错误与安全协议类型有关，很可能是由于您的应用程序的默认安全协议类型设置得太低，现在很多外部 API 都期望使用 TLS 1.2 或更高版本的请求。
            //这可以在您调用 api 之前添加，甚至可以在应用程序启动方法中添加。
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<DownloadService>().ToSelf().InSingletonScope();
            builder.Bind<SettingsService>().ToSelf().InSingletonScope();
            builder.Bind<SoundsService>().ToSelf().InSingletonScope();

            builder.Bind<IViewModelFactory>().ToAbstractFactory();
        }

#if !DEBUG
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            base.OnUnhandledException(e);

            MessageBox.Show(e.Exception.Message, "错误详情", MessageBoxButton.OK, MessageBoxImage.Error);
        }
#endif
    }
}
