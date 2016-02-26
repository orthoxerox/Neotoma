using System;
using System.Collections.Generic;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Recursive : Pattern
    {
        public Pattern Pattern
        {
            get; set;
        }

        public Recursive(
            string name = null)
            : base (name)
        {
        }

        internal Recursive(
            Pattern pattern,
            string name = null)
            : base(name)
        {
            Pattern = pattern;
        }

        protected override ParsingResult InternalMatch(
            Position position, 
            IMemo memo)
        {
            return Pattern.Match(position, memo);
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Recursive(Pattern, name);
        }

        public override string ToString()
        {
            return Pattern.ToString();
        }
    }
}