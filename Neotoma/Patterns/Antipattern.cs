using System;
using System.Collections.Generic;

namespace Neotoma
{
    using System.Diagnostics.Contracts;
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Antipattern : Pattern
    {
        public Pattern Pattern
        {
            get;
        }

        public Antipattern(
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
                return new ParsingError(
                    $"Successfully matched antipattern",
                    position,
                    this);
            } else {
                return new ParseNode(this, position, position);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Antipattern(Pattern, name);
        }

        public override string ToString()
        {
            return "!" + Pattern.ToNestedString();
        }
    }
}