using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public class ClosedCaption
    {
        public string Content { get; }
        public TimeSpan From { get; }
        public TimeSpan To { get; }

        public ClosedCaption(string content,TimeSpan from,TimeSpan to)
        {
            Content = content;
            From = from;
            To = to;
        }
    }
}
