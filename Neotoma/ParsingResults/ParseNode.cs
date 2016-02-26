using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Neotoma
{
    public class ParseNode : ParsingResult
    {
        private static readonly IReadOnlyList<ParseNode> empty = new List<ParseNode>(0);

        public IReadOnlyList<ParseNode> Children { get; }
        public Position NextPosition { get; }
        public int Length { get; }
        public string Value { get; }
        public string Name { get; }

        public ParseNode(
            Pattern pattern,
            Position startPosition, 
            Position nextPosition) 
            : base(startPosition)
        {
            Contract.Requires<ArgumentException>(
                startPosition.String == nextPosition.String);
            Contract.Requires<ArgumentException>(
                startPosition.Index <= nextPosition.Index);

            Children = empty;
            Length = nextPosition.Index - startPosition.Index;
            NextPosition = nextPosition;
            Value = Position.String.Substring(Position.Index, Length);
            Name = GetNodeNameFromPattern(pattern);
        }

        public ParseNode(
            Pattern pattern,
            params ParseNode[] children) 
            : base(children[0].Position)
        {
            Children = children;
            Length = Children.Sum(c => c.Length);
            NextPosition = children.Last().NextPosition;
            Value = default(string);
            Name = GetNodeNameFromPattern(pattern);
        }

        public ParseNode(
            Pattern pattern,
            IReadOnlyList<ParseNode> children) 
            : base(children?[0].Position ?? default(Position))
        {
            Contract.Requires<ArgumentNullException>(children != null);
            Contract.Requires<ArgumentException>(children.Count > 0);

            Children = children;
            Length = Children.Sum(c => c.Length);
            NextPosition = children.Last().NextPosition;
            Value = default(string);
            Name = GetNodeNameFromPattern(pattern);
        }

        private static string GetNodeNameFromPattern(Pattern pattern)
            => pattern.Named ? pattern.Name : null;
    }
}