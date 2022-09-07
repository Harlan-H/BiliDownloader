using System.Diagnostics.CodeAnalysis;

namespace BiliDownloader.Core.Common
{
    public class Author
    {
        public string? Title { get; }

        public Author(string? title)
        {
            Title = title;
        }

        [ExcludeFromCodeCoverage]
        public override string ToString() => Title!;
    }
}
