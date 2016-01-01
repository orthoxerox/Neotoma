using System;
using System.Collections.Generic;
using System.Globalization;

namespace Neotoma
{
    public class SingleCharacter : SimplePattern
    {
        public UnicodeCategory Category
        {
            get;
        }

        public SingleCharacter(
            UnicodeCategory category, 
            bool memoized = false,
            string name = null) 
            : base (memoized, name)
        {
            Category = category;
        }

        protected override ParsingResult InternalInternalMatch(Position position, IDictionary<Tuple<Pattern, Position>, ParsingResult> memo)
        {
            var c = position.String[position.Index];
            var cat = char.GetUnicodeCategory(c);
            if (cat == Category) {
                return new ParseNode(position, position.Advance());
            } else {
                return new ParsingError(
                    $"Expected {Category}, got {cat}", 
                    position, 
                    this);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new SingleCharacter(Category, true);
        }

        public override string ToString()
        {
            return $"[[{Category.ToString()}]]";
        }
    }
}