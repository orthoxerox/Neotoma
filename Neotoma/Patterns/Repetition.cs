using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Repetition : Pattern
    {
        public Pattern Pattern { get; }
        public int Minimum { get; }
        public int Maximum { get; }

        public Repetition(
            Pattern pattern, 
            int minimum, 
            int maximum, 
            bool memoized = false,
            string name = null) 
            : base (memoized, name)
        {
            Contract.Requires(pattern != null);
            Contract.Requires(minimum >= 0);
            Contract.Requires(maximum >= minimum);
            Contract.Requires(!(pattern is Antipattern));
            Contract.Requires(!(pattern is Backtrack));

            //TODO: repetition of repetition

            Pattern = pattern;
            Minimum = minimum;
            Maximum = maximum;
        }

        protected override ParsingResult InternalMatch(
            Position position, 
            IMemo memo)
        {
            var nodes = new List<ParseNode>();
            var totalLength = 0;
            var pos = position;

            for (int i = 0; i < Maximum; i++) {
                var result = Pattern.Match(pos, memo);
                var node = result as ParseNode;

                if (node != null) {
                    nodes.Add(node);
                    totalLength += node.Length;
                    pos = node.NextPosition;
                } else {
                    if (i >= Minimum) {
                        break;
                    } else {
                        return new ParsingError(
                            $"Found only {i} repetitions instead of {Minimum}",
                            pos,
                            this);
                    }
                }
            }
            if (Pattern.Memoized) {
                return new ParseNode(nodes);
            } else {
                return new ParseNode(position, pos);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Repetition(Pattern, Minimum, Maximum, true);
        }

        public override string ToString()
        {
            if (Minimum == 0 && Maximum == 1)
                return Pattern.ToNestedString() + "?";

            if (Minimum == 0 && Maximum == int.MaxValue)
                return Pattern.ToNestedString() + "*";

            if (Minimum == 1 && Maximum == int.MaxValue)
                return Pattern.ToNestedString() + "+";

            return $"{Pattern.ToNestedString()}{{{Minimum},{Maximum}}}";
        }
    }
}