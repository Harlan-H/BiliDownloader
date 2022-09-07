using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Utils.Extensions
{
    internal static class CollectionExtensions
    {
        public static void RemoveWhere<T>(this ICollection<T> source, Predicate<T> predicate)
        {
            foreach (var i in source.ToArray())
            {
                if (predicate(i))
                    source.Remove(i);
            }
        }
    }
}
