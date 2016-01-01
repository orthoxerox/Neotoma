using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Neotoma
{
    public static class Helpers
    {
        public static Literal p(string value)
            => new Literal(value);

        public static Set s(string chars)
        {
            Contract.Requires(!string.IsNullOrEmpty(chars));
            return new Set(chars.ToCharArray());
        }

        public static Range r(char lo, char hi)
            => new Range(lo, hi);

        public static readonly Pattern Any = new AnySingleCharacter();

        public static readonly Pattern EOF = !Any;

        public static readonly Pattern Lowercase
            = new SingleCharacter(UnicodeCategory.LowercaseLetter);

        public static readonly Pattern Uppercase
            = new SingleCharacter(UnicodeCategory.UppercaseLetter);

        public static readonly Pattern DecimalDigit
            = new SingleCharacter(UnicodeCategory.DecimalDigitNumber);
    }
}
