using System;
using System.Collections.Generic;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class AnySingleCharacter : SimplePattern
    {
        public AnySingleCharacter(
            string name = null)
            : base(name)
        {
        }

        protected override ParsingResult InternalInternalMatch(
            Position position, 
            IMemo memo)
        {
            return new ParseNode(this, position, position.Advance());
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new AnySingleCharacter(name);
        }

        public override string ToString()
        {
            return ".";
        }
    }
}