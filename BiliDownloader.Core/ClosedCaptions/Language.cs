using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.Core.ClosedCaptions
{
    public readonly partial struct Language
    {
        public string Code { get; }
        public string Name { get; }

        public Language(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public override string ToString() => $"{Code} - {Name}";
    }

    public readonly partial struct Language : IEquatable<Language>
    {
        public bool Equals(Language other) => StringComparer.OrdinalIgnoreCase.Equals(Code, other.Code);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Code);

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Language other && Equals(other);

        public static bool operator ==(Language left, Language right) => left.Equals(right);

        public static bool operator !=(Language left, Language right) => !(left == right);
    }
}
