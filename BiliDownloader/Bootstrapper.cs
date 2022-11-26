using BiliDownloader.Services;
using BiliDownloader.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace BiliDownloader
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _simpleContainer = new();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _simpleContainer
               .PerRequest<IWindowManager, WindowManager>();

            _simpleContainer
                .Singleton<DownloadService>()
                .Singleton<SoundsService>()
                .Singleton<QueryService>()
                .Singleton<SettingsService>()
                .PerRequest<MainWindowViewModel>()
                .PerRequest<DownloadViewModel>()
                .PerRequest<DownloadMultipleSetupViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _simpleContainer.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _simpleContainer.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _simpleContainer.BuildUp(instance);
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            //The ssl connection could not be established,see inner exception.
            //https://docs.microsoft.com/en-us/answers/questions/400152/authentication-failed-because-the-remote-party-has.html
            //主要的原因是 尝试调用外部 API 时可能会出现此错误。此错误与安全协议类型有关，很可能是由于您的应用程序的默认安全协议类型设置得太低，现在很多外部 API 都期望使用 TLS 1.2 或更高版本的请求。
            //这可以在您调用 api 之前添加，甚至可以在应用程序启动方法中添加。
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            HttpClient.DefaultProxy = new WebProxy();
            ServicePointManager.DefaultConnectionLimit = 40;
            await DisplayRootViewForAsync<MainWindowViewModel>();
        }

#if !DEBUG
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            base.OnUnhandledException(sender, e);

            MessageBox.Show(e.Exception.Message, "错误详情", MessageBoxButton.OK, MessageBoxImage.Error);
        }
#endif
    }
}
