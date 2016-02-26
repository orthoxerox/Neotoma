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
            string name = null) 
            : base(name)
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
            return new ParseNode(this, position, pos);
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Literal(Value, name);
        }

        public override string ToString()
        {
            return Value.ToRoundtripString();
        }
    }
}