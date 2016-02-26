using System;
using System.Collections.Generic;
using System.Globalization;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class SingleCharacter : SimplePattern
    {
        public UnicodeCategory Category
        {
            get;
        }

        public SingleCharacter(
            UnicodeCategory category, 
            string name = null) 
            : base (name)
        {
            Category = category;
        }

        protected override ParsingResult InternalInternalMatch(Position position, IMemo memo)
        {
            var c = position.String[position.Index];
            var cat = char.GetUnicodeCategory(c);
            if (cat == Category) {
                return new ParseNode(this, position, position.Advance());
            } else {
                return new ParsingError(
                    $"Expected {Category}, got {cat}", 
                    position, 
                    this);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new SingleCharacter(Category, name);
        }

        public override string ToString()
        {
            return $"[[{Category.ToString()}]]";
        }
    }
}