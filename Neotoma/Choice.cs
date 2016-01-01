using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Neotoma
{
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
            bool memoized = false,
            string name = null) : base(memoized, name)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);

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

        public Choice(
            IReadOnlyList<Pattern> patterns,
            bool memoized = false,
            string name = null)
            : base(memoized, name)
        {
            Contract.Requires(patterns != null);
            Contract.Requires(patterns.Count > 0);
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
                "Failed to match any choice",
                position,
                this);
        }

        protected override Pattern InternalMemoize(string name)
        {
            return new Choice(Patterns, true, name);
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