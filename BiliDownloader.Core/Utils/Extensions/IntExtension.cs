using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    public static class IntExtension
    {
        public static double? TryConvertDoubleOrNull(this int element) =>
             Convert.ToDouble(element);
    }
}
