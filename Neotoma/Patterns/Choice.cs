using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Neotoma
{
    using System.Linq;
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public class Choice : Pattern
    {
        public IReadOnlyList<Pattern> Patterns
        {
            get;
        }

        public Choice(
            Pattern first,
            Pattern second,
            string name = null) : base(name)
        {
            Contract.Requires<ArgumentNullException>(first != null);
            Contract.Requires<ArgumentNullException>(second != null);

            var firstchoice = first as Choice;
            var secondchoice = second as Choice;
            var count = firstchoice?.Patterns.Count ?? 1 + secondchoice?.Patterns.Count ?? 1;
            var patterns = new List<Pattern>(count);

            if (firstchoice != null) {
                patterns.AddRange(firstchoice.Patterns);
            } else {
                patterns.Add(first);
            }

            if (secondchoice != null) {
                patterns.AddRange(secondchoice.Patterns);
            } else {
                patterns.Add(second);
            }

            Patterns = patterns;
        }

        public Choice(params Pattern[] patterns)
            : this(patterns, null)
        { }

        public Choice(
            IReadOnlyList<Pattern> patterns,
            string name = null)
            : base(name)
        {
            Contract.Requires<ArgumentNullException>(patterns != null);
            Contract.Requires<ArgumentException>(patterns.Count > 0);
            Contract.Requires<ArgumentNullException>(patterns.All(p=>p!=null));
            var ps = new List<Pattern>(patterns.Count);
            foreach (var p in patterns) {
                var c = p as Choice;
                if (c != null) {
                    ps.AddRange(c.Patterns);
                } else {
                    ps.Add(p);
                }
            }
            Patterns = ps;
        }

        //Added to skip redundant checks
        private Choice(
            string name,
            IReadOnlyList<Pattern> patterns)
            : base(name)
        {
            Patterns = patterns;
        }

        protected override ParsingResult InternalMatch(
            Position position, 
            IMemo memo)
        {
            foreach (var pattern in Patterns) {
                var result = pattern.Match(position, memo);
                var node = result as ParseNode;

                if (node != null) {
                    return node;
                }
            }

            return new ParsingError(
                "Failed to match any patern",
                position,
                this);
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Choice(name, Patterns);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var p in Patterns) {
                sb.Append(p.ToNestedString());
                sb.Append(" / ");
            }
            sb.Length -= 3;
            return sb.ToString();
        }
    }
}