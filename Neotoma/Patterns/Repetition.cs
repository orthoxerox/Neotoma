using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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
            string name = null) 
            : base (name)
        {
            Contract.Requires<ArgumentNullException>(pattern != null);
            Contract.Requires<ArgumentOutOfRangeException>(minimum >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(maximum >= minimum);
            Contract.Requires<ArgumentException>(!(pattern is Antipattern));
            Contract.Requires<ArgumentException>(!(pattern is Backtrack));

            //TODO: repetition of repetition

            Pattern = pattern;
            Minimum = minimum;
            Maximum = maximum;
        }

        /*
        Some notes on the ParseNode creation.

        Our current pattern can be 
        a) Named 
        b) not Named
        Inner pattern can return the following nodes: 
        1) Named with no children
        2) Named with children
        3) not Named with no children
        4) not Named with children

        We can say that it makes no sense to have a node with children, unless the children are named

        Therefore a pattern will have only children that are named. We flatten the nodes that are nameless.
        */
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
                    if (node.Name != null) {
                        nodes.Add(node);
                    } else {
                        //if there are no children, we add nothing at all
                        nodes.AddRange(node.Children.Where(n => n.Name != null));
                    }
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
            if (nodes.Count > 0) {
                if (!Named && nodes.Count == 1) {
                    return nodes[0];
                } else {
                    return new ParseNode(this, nodes);
                }
            } else {
                return new ParseNode(this, position, pos);
            }
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Repetition(Pattern, Minimum, Maximum, name);
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