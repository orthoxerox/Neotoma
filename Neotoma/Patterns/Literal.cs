using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Literal : SimplePattern
    {
        public string Value
        {
            get;
        }

        public Literal(
            string value, 
            bool memoized = false,
            string name = null) 
            : base(memoized, name)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(value));
            Value = value;
        }

        protected override ParsingResult InternalInternalMatch(
            Position position, 
            IMemo memo)
        {
            var pos = position;
            for (int i = 0; i < Value.Length; i++) {
                if (pos.EOF) {
                    return new ParsingError(
                        $"Expected \"{Value}\", got EOF",
                        pos,
                        this);
                }
                var exp = Value[i];
                var rec = pos.String[pos.Index];
                if (exp != rec) {
                    return new ParsingError(
                        $"Expected \"{Value}\", got '{rec}' instead of '{exp}'",
                        pos,
                        this);
                }
                pos = pos.Advance();
            }
            return new ParseNode(position, pos);
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Literal(Value, true, name);
        }

        public override string ToString()
        {
            return Value.ToRoundtripString();
        }
    }
}