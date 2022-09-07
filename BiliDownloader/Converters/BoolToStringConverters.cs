using System;
using System.Globalization;

namespace BiliDownloader.Converters
{
    public class BoolToSkipStringConverters : BaseConverters<bool, string>
    {
        public static BoolToSkipStringConverters Instance { get; } = new BoolToSkipStringConverters();
        public override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? "跳过" : "覆盖";
        }

        public override bool ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
