using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Range : SimplePattern
    {
        public char Low
        {
            get;
        }

        public char High
        {
            get;
        }

        public Range(
            char low, 
            char high, 
            bool memoized = false,
            string name = null) 
            : base (memoized, name)
        {
            Contract.Requires<ArgumentException>(low <= high);
            Low = low;
            High = high;
        }

        protected override ParsingResult InternalInternalMatch(
            Position position, 
            IMemo memo)
        {
            var c = position.String[position.Index];
            if (Low <= c && c <= High) {
                return new ParseNode(position, position.Advance());
            } else {
                return new ParsingError(
                    $"Character '{c}' not in set {this}",
                    position,
                    this);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Range(Low, High, true, name);
        }

        public override string ToString()
        {
            return $"[{Low.ToRoundtripString()}-{High.ToRoundtripString()}]";
        }
    }
}