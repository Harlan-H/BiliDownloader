using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Exceptions
{
    class DownloaderException : Exception
    {
        public DownloaderException(string message) : base(message)
        {

        }
    }
}
