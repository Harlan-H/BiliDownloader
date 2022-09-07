using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Utils
{
    internal static class Memory
    {
        private static class Mem<T>
        {
            private static readonly ConditionalWeakTable<object, Dictionary<int, T>> CacheManifest = new();

            public static Dictionary<int, T> GetCache(object owner) => CacheManifest.GetOrCreateValue(owner);
        }

        public static T Cache<T>(object owner, Func<T> valueFunc)
        {
            var cache = Mem<T>.GetCache(owner);
            var key = valueFunc.Method.GetHashCode();

            if (cache.TryGetValue(key, out T? cachedValue))
                return cachedValue;

            var value = valueFunc();
            cache[key] = value;

            return value;
        }
    }
}
