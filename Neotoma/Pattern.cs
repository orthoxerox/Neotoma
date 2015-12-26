using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Neotoma
{
    public abstract class Pattern
    {
        internal Pattern()
        {

        }

        public static Sequence operator +(
            Pattern lhs,
            Pattern rhs)
        => new Sequence(lhs, rhs);

        public static Choice operator /(
            Pattern lhs,
            Pattern rhs)
        => new Choice(lhs, rhs);

        public static Antipattern operator !(
            Pattern rhs)
        => new Antipattern(rhs);

        public static Sequence operator -(
            Pattern lhs,
            Pattern rhs)
        => !rhs + lhs;

        public static Repetition operator ~(
            Pattern rhs)
        => new Repetition(rhs, 0, 1);

        public static Repetition operator *(
            Pattern lhs,
            int rhs)
        => new Repetition(lhs, 1, rhs);

        public static Sequence operator &(
            Pattern lhs, 
            Pattern rhs)
        => lhs + new Backtrack(rhs);
    }

    public class Literal : Pattern
    {
        public string Value { get; }

        public Literal(string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(value));

            Value = value;
        }
    }

    public class Set : Pattern
    {
        public IReadOnlyList<char> Values { get; }

        public Set(params char[] values)
        {
            Contract.Requires(values != null);
            Contract.Requires(values.Length > 0);

            Values = values;
        }
    }

    public class Range : Pattern
    {
        public char Low { get; }
        public char High { get; }

        public Range(char low, char high)
        {
            Contract.Requires(low <= high);

            Low = low;
            High = high;
        }
    }

    public class Sequence : Pattern
    {
        public IReadOnlyList<Pattern> Patterns { get; }

        public Sequence(Pattern first, Pattern second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);

            var firstseq = first as Sequence;
            var secondseq = second as Sequence;

            var count =
                firstseq?.Patterns.Count ?? 1 +
                secondseq?.Patterns.Count ?? 1;

            var patterns = new List<Pattern>(count);

            if (firstseq != null) {
                patterns.AddRange(firstseq.Patterns);
            } else {
                patterns.Add(first);
            }

            if (secondseq != null) {
                patterns.AddRange(secondseq.Patterns);
            } else {
                patterns.Add(second);
            }

            Patterns = patterns;
        }
    }

    public class Choice : Pattern
    {
        public IReadOnlyList<Pattern> Patterns { get; }

        public Choice(Pattern first, Pattern second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);

            var firstchoice = first as Choice;
            var secondchoice = second as Choice;

            var count =
                firstchoice?.Patterns.Count ?? 1 +
                secondchoice?.Patterns.Count ?? 1;

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
    }

    public class Repetition : Pattern
    {
        public Pattern Pattern { get; }
        public int Minimum { get; }
        public int Maximum { get; }

        public Repetition(
            Pattern pattern,
            int minimum,
            int maximum)
        {
            Contract.Requires(pattern != null);
            Contract.Requires(minimum >= 0);
            Contract.Requires(maximum >= minimum);

            Pattern = pattern;
            Minimum = minimum;
            Maximum = maximum;
        }
    }

    public class Antipattern : Pattern
    {
        public Pattern Pattern { get; }

        public Antipattern(Pattern pattern)
        {
            Pattern = pattern;
        }
    }

    public class Backtrack : Pattern
    {
        public Pattern Pattern { get; }

        public Backtrack(Pattern pattern)
        {
            Pattern = pattern;
        }
    }
}
