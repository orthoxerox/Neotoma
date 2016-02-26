using System.Linq;
using System.Collections.Generic;
using System;

namespace Neotoma
{
    using Memo = Dictionary<Tuple<Pattern, Position>, ParsingResult>;
    using IMemo = IDictionary<Tuple<Pattern, Position>, ParsingResult>;

    public abstract class Pattern
    {
        public bool Memoized => Name != null;

        public bool Named => !string.IsNullOrEmpty(Name);
        
        public string Name { get; }

        internal Pattern(string name)
        {
            Name = name;
        }

        public Pattern Memoize(string name = "")
        {
            if (Name == name) {
                return this;
            }

            return InternalMemoize(name);
        }

        protected abstract Pattern InternalMemoize(string name);
        public ParsingResult Match(string input)
        {
            return Match(new Position(input), new Memo());
        }

        internal ParsingResult Match(Position position, IMemo memo)
        {
            ParsingResult result;
            if (memo.TryGetValue(Tuple.Create(this, position), out result)) {
                return result;
            }

            return InternalMatch(position, memo);
        }

        protected abstract ParsingResult InternalMatch(Position position, IDictionary<Tuple<Pattern, Position>, ParsingResult> memo);
        public virtual string ToNestedString() => $"({ToString()})";

        public Pattern this[int min, int max]
        {
            get
            {
                return new Repetition(this, min, max);
            }
        }

        public Pattern this[int min]
        {
            get
            {
                return new Repetition(this, min, int.MaxValue);
            }
        }

        public static Pattern operator +(Pattern lhs, Pattern rhs)
        {
            if (lhs.Memoized || rhs.Memoized)
                return new Sequence(lhs, rhs);

            var llit = lhs as Literal;
            var rrit = rhs as Literal;

            if (llit != null && rrit != null) {
                return new Literal(llit.Value + rrit.Value);
            }

            return new Sequence(lhs, rhs);
        }

        public static Pattern operator /(Pattern lhs, Pattern rhs)
        {
            if (lhs.Memoized || rhs.Memoized)
                return new Sequence(lhs, rhs);

            var lset = lhs as Set;
            var rset = rhs as Set;

            if (lset != null && rset != null) {
                return new Set(lset.Values.Concat(rset.Values).ToArray());
            }

            return new Choice(lhs, rhs);
        }

        public static Antipattern operator !(Pattern rhs) => new Antipattern(rhs);
        public static Pattern operator -(Pattern lhs, Pattern rhs) => !rhs + lhs;
        public static Repetition operator ~(Pattern rhs) => new Repetition(rhs, 0, 1);
        public static Repetition operator *(Pattern lhs, int rhs) => new Repetition(lhs, 1, rhs);
        public static Pattern operator &(Pattern lhs, Pattern rhs) => lhs + new Backtrack(rhs);
        public static implicit operator Pattern(string rhs) => new Literal(rhs);
    }
}