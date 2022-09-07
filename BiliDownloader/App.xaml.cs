using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BiliDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Assembly Assembly { get; } = typeof(App).Assembly;
        public static Version Version { get; } = Assembly.GetName().Version!;
        public static string VersionString { get; } = Version.ToString(3);
    }

    public partial class App
    {
        private static Theme LightTheme { get; } = Theme.Create(
            new MaterialDesignLightTheme(),
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#343838"),
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F9A825")
        );

        private static Theme DarkTheme { get; } = Theme.Create(
            new MaterialDesignDarkTheme(),
           (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E8E8E8"),
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F9A825")
        );

        public static void SetLightTheme()
        {
            var paletteHelper = new PaletteHelper();
            paletteHelper.SetTheme(LightTheme);

            Current.Resources["SuccessBrush"] = new SolidColorBrush(Colors.DarkGreen);
            Current.Resources["CanceledBrush"] = new SolidColorBrush(Colors.DarkOrange);
            Current.Resources["FailedBrush"] = new SolidColorBrush(Colors.DarkRed);
        }

        public static void SetDarkTheme()
        {
            var paletteHelper = new PaletteHelper();
            paletteHelper.SetTheme(DarkTheme);

            Current.Resources["SuccessBrush"] = new SolidColorBrush(Colors.LightGreen);
            Current.Resources["CanceledBrush"] = new SolidColorBrush(Colors.Orange);
            Current.Resources["FailedBrush"] = new SolidColorBrush(Colors.OrangeRed);
        }
    }
}
