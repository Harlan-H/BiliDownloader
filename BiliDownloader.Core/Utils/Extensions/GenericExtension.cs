using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    internal static class GenericExtension
    {
        public static TOut Pipe<TIn, TOut>(this TIn input, Func<TIn, TOut> transform) => transform(input);



        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> =>
            value.CompareTo(min) <= 0
            ? min
            : value.CompareTo(max) >= 0
                ? max
                : value;
    }
}
