using System;
using System.Collections.Generic;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class AnySingleCharacter : SimplePattern
    {
        public AnySingleCharacter(
            bool memoized = false,
            string name = null)
            : base(memoized, name)
        {
        }

        protected override ParsingResult InternalInternalMatch(
            Position position, 
            IMemo memo)
        {
            return new ParseNode(position, position.Advance());
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new AnySingleCharacter(true, name);
        }

        public override string ToString()
        {
            return ".";
        }
    }
}