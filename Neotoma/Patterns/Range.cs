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
            string name = null) 
            : base (name)
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
                return new ParseNode(this, position, position.Advance());
            } else {
                return new ParsingError(
                    $"Character '{c}' not in set {this}",
                    position,
                    this);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Range(Low, High, name);
        }

        public override string ToString()
        {
            return $"[{Low.ToRoundtripString()}-{High.ToRoundtripString()}]";
        }
    }
}