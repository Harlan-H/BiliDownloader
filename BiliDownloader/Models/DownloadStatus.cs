using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Models
{
    public enum DownloadStatus
    {
        Downloaded,
        Enqueued,
        Completed,
        Failed,
        Canceled
    }
}
