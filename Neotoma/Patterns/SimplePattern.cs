using System;
using System.Collections.Generic;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public abstract class SimplePattern : Pattern
    {
        internal SimplePattern(string name) : base(name)
        {
        }

        protected override sealed ParsingResult InternalMatch(Position position, IMemo memo)
        {
            if (position.EOF) {
                return new ParsingError("EOF reached", position, this);
            } else {
                return InternalInternalMatch(position, memo);
            }
        }

        protected abstract ParsingResult InternalInternalMatch(Position position, IMemo memo);
        public override string ToNestedString() => ToString();
    }
}