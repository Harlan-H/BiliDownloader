using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils.Extensions
{
    internal static class CollectionExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
        {
            foreach (var i in source)
            {
                if (i is not null)
                    yield return i;
            }
        }
    }
}
