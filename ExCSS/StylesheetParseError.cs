namespace ExCSS
{
    public class StylesheetParseError
    {
        public StylesheetParseError(ParserError error, int line, int column, string message)
        {
            ParserError = error;
            Line = line;
            Column = column;
            Message = message ?? "";
        }

        public ParserError ParserError { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0} Line {1}, Column {2}", Message, Line, Column);
        }
    }
}