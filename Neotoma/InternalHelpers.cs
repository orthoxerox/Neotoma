using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neotoma
{
    internal static class InternalHelpers
    {
        [Pure]
        public static string ToRoundtripString(this char c)
        {
            switch (c) {
                case '\r': return @"\r";
                case '\n': return @"\n";
                case '\\': return @"\\";
                case '"': return @"\""";
                case '\0': return @"\0";
                case '\a': return @"\a";
                case '\b': return @"\b";
                case '\f': return @"\f";
                case '\t': return @"\t";
                case '\v': return @"\v";
                default: return c.ToString();
            }

        }

        [Pure]
        public static string ToRoundtripString(this string s)
        {
            var sb = new StringBuilder(s.Length+2);
            sb.Append("\"");
            foreach (var c in s) {
                sb.Append(c.ToRoundtripString());
            }
            sb.Append("\"");
            return sb.ToString();
        }
    }
}
