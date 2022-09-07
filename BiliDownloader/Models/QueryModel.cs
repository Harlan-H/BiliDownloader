using BiliDownloader.Core.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Models
{
    public class QueryModel
    {
        public object Id { get; }
        public IVideo Value { get; }

        public QueryModel(object id, IVideo value)
        {
            Id = id;
            Value = value;
        }
    }
}
