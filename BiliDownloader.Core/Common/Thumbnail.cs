using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Common
{
    public class Thumbnail
    {
        public string  Url { get; }

        public Thumbnail(string url)
        {
            Url = url;
        }

        public override string ToString() => Url;
    }
}
