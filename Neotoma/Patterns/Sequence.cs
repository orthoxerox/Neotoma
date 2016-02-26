using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Neotoma
{
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Sequence : Pattern
    {
        public IReadOnlyList<Pattern> Patterns
        {
            get;
        }

        public Sequence(
            Pattern first, 
            Pattern second, 
            string name = null) 
            : base (name)
        {
            Contract.Requires<ArgumentNullException>(first != null);
            Contract.Requires<ArgumentNullException>(second != null);
            var firstseq = first as Sequence;
            var secondseq = second as Sequence;
            var count = firstseq?.Patterns.Count ?? 1 + secondseq?.Patterns.Count ?? 1;
            var patterns = new List<Pattern>(count);
            if (firstseq != null && !first.Memoized)
            {
                patterns.AddRange(firstseq.Patterns);
            }
            else
            {
                patterns.Add(first);
            }

            if (secondseq != null && !second.Memoized)
            {
                patterns.AddRange(secondseq.Patterns);
            }
            else
            {
                patterns.Add(second);
            }

            Patterns = patterns;
        }

        public Sequence(
            IReadOnlyList<Pattern> patterns, 
            string name = null) 
            : base (name)
        {
            Contract.Requires<ArgumentNullException>(patterns != null);
            Contract.Requires<ArgumentException>(patterns.Count > 0);
            Patterns = patterns;
        }

        protected override ParsingResult InternalMatch(
            Position position, 
            IMemo memo)
        {
            var nodes = new List<ParseNode>();
            var pos = position;
            var lastPattern = default(Pattern);
            var lastNode = default(ParseNode);

            foreach (var pattern in Patterns) {
                var result = pattern.Match(pos, memo);
                var node = result as ParseNode;

                if (node != null) {
                    if (node.Name != null) {
                        nodes.Add(node);
                    } else {
                        //if there are no children, we add nothing at all
                        nodes.AddRange(node.Children.Where(n => n.Name != null));
                    }
                    pos = node.NextPosition;
                } else {
                    return result; //TODO: nested errors
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
            return new Sequence(Patterns, name);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var p in Patterns) {
                sb.Append(p.ToNestedString());
                sb.Append(' ');
            }
            sb.Length--;
            return sb.ToString();
        }
    }
}