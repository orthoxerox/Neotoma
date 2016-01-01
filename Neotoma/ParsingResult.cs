using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neotoma
{
    public abstract class ParsingResult
    {
        public Position Position { get; }

        internal ParsingResult(Position position)
        {
            Position = position;
        }
    }

    public class ParsingError : ParsingResult
    {
        public string Message { get; }
        public Pattern Pattern { get; }

        public ParsingError(
            string message,
            Position position,
            Pattern pattern)
            : base(position)
        {
            Message = message;
            Pattern = pattern;
        }

    }

    public class ParseNode : ParsingResult
    {
        private static readonly IReadOnlyList<ParseNode> empty
            = new List<ParseNode>(0);

        public IReadOnlyList<ParseNode> Children { get; }
        public Position NextPosition { get; }
        public int Length { get; }
        public string Value { get; }

        public ParseNode(
            Position startPosition, 
            Position nextPosition) : base(startPosition)
        {
            Children = empty;
            Length = nextPosition.Index - startPosition.Index;
            NextPosition = nextPosition;
            Value = Position.String.Substring(Position.Index, Length);
        }

        public ParseNode(params ParseNode[] children)
            : base(children[0].Position)
        {
            Children = children;
            Length = Children.Sum(c=>c.Length);
            NextPosition = children.Last().NextPosition;
            Value = default(string);
        }

        public ParseNode(IReadOnlyList<ParseNode> children)
            : base(children?[0].Position ?? default(Position))
        {
            Contract.Requires(children != null);
            Contract.Requires(children.Count > 0);

            Children = children;
            Length = Children.Sum(c => c.Length);
            NextPosition = children.Last().NextPosition;
            Value = default(string);
        }
    }
}
