using BiliDownloader.Core.Utils;
using BiliDownloader.Core.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiliDownloader.Core.Extractors
{
    internal class ClosedCaptionExtractor
    {
        private readonly JsonElement jsonElement;

        public ClosedCaptionExtractor(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public TimeSpan? TryGetFrom() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("from")?
            .GetDoubleOrNull()?
            .Pipe(TimeSpan.FromSeconds)
        );

        public TimeSpan? TryGetTo() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("to")?
            .GetDoubleOrNull()?
            .Pipe(TimeSpan.FromSeconds)
        );

        public string? TryGetText() => Memory.Cache(this, () =>
            jsonElement
            .GetPropertyOrNull("content")?
            .GetStringOrNull()
        );


    }
}
