namespace ExCSS
{
    public class StylesheetParseError
    {
        public StylesheetParseError(ParserError error, int line, int column, string message)
        {
            ParserError = error;
            Line = line;
            Column = column;
            Message = message;
        }

        public ParserError ParserError { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; }
    }
}