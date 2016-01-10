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
}