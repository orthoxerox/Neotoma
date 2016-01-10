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
            bool memoized = false,
            string name = null)
            : base (memoized, name)
        {
        }

        internal Recursive(
            Pattern pattern,
            bool memoized = false,
            string name = null)
            : base(memoized, name)
        {
            Pattern = pattern;
        }

        protected override ParsingResult InternalMatch(
            Position position, 
            IMemo memo)
        {
            return Pattern.Match(position, memo) as ParseNode;
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Recursive(Pattern, true, name);
        }

        public override string ToString()
        {
            return Pattern.ToString();
        }
    }
}