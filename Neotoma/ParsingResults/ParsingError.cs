namespace Neotoma
{
    public class ParsingError : ParsingResult
    {
        public string Message
        {
            get;
        }

        public Pattern Pattern
        {
            get;
        }

        public ParsingError InnerError
        {
            get;
        }

        public ParsingError(string message, Position position, Pattern pattern, ParsingError innerError = null): base (position)
        {
            Message = message;
            Pattern = pattern;
            if (innerError != null && innerError.Pattern.Name != null)
            {
                InnerError = innerError;
            }
        }
    }
}