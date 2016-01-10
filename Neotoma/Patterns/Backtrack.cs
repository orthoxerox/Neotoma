using System;
using System.Collections.Generic;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Backtrack : Pattern
    {
        public Pattern Pattern
        {
            get;
        }

        public Backtrack(
            Pattern pattern, 
            bool memoized = false,
            string name = null)
            : base (memoized, name)
        {
            Pattern = pattern;
        }

        protected override ParsingResult InternalMatch(
            Position position,
            IMemo memo)
        {
            var result = Pattern.Match(position, memo) as ParseNode;

            if (result != null) {
                return new ParseNode(position, position);
            } else {
                return new ParsingError(
                    $"Successfully matched antipattern",
                    position,
                    this);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Backtrack(Pattern, true, name);
        }

        public override string ToString()
        {
            return "&" + Pattern.ToNestedString();
        }
    }
}