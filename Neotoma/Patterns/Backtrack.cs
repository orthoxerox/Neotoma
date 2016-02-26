using System;
using System.Collections.Generic;

namespace Neotoma
{
    using System.Diagnostics.Contracts;
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Backtrack : Pattern
    {
        public Pattern Pattern
        {
            get;
        }

        public Backtrack(
            Pattern pattern, 
            string name = null)
            : base (name)
        {
            Contract.Requires<ArgumentNullException>(pattern != null);
            Pattern = pattern;
        }

        protected override ParsingResult InternalMatch(
            Position position,
            IMemo memo)
        {
            var result = Pattern.Match(position, memo) as ParseNode;

            if (result != null) {
                return new ParseNode(this, position, position);
            } else {
                return new ParsingError(
                    $"Couldn't match pattern {Pattern}",
                    position,
                    this);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Backtrack(Pattern, name);
        }

        public override string ToString()
        {
            return "&" + Pattern.ToNestedString();
        }
    }
}