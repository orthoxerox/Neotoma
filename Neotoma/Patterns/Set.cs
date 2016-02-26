using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Set : SimplePattern
    {
        public ISet<char> Values
        {
            get;
        }

        public Set(params char[] values) : base(null)
        {
            Contract.Requires< ArgumentNullException>(values != null);
            Contract.Requires<ArgumentException>(values.Length > 0);
            var set = new HashSet<char>(values);
            Values = set;
        }

        public Set(
            ISet<char> values, 
            string name = null)
            : base (name)
        {
            Contract.Requires< ArgumentNullException>(values != null);
            Contract.Requires<ArgumentException>(values.Count > 0);
            Values = values;
        }

        protected override ParsingResult InternalInternalMatch(
            Position position, 
            IMemo memo)
        {
            var c = position.String[position.Index];
            if (Values.Contains(c)) {
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
            return new Set(Values, name);
        }

        public override string ToString()
        {
            var chars = Values.Aggregate(
                new System.Text.StringBuilder(),
                (sb, c) => sb.Append(c.ToRoundtripString())
                ).ToString();
            return $"[{chars}]";
        }
    }
}