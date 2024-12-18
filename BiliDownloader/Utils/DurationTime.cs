using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Utils
{
    public class DurationTime(long filesize)
    {
        private long LastBytes { get; set; }
        private readonly long Filesize = filesize;

        private double Next(long currentpos)
        {
            if (Filesize == 0)
            {
                return 0;
            }
            else
            {
                long Bytes = currentpos - LastBytes;
                LastBytes = currentpos;

                return Bytes <= 0 ? 0 : (Filesize - currentpos) / Bytes;
            }
        }

        public TimeSpan GetNext(long currentpos)
        {
            double ret = Next(currentpos);
            return TimeSpan.FromSeconds(ret);
        }
    }
}
