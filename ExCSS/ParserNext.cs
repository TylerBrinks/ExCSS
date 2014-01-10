using System.IO;

namespace ExCSS
{
    public class ParserNext
    {
        private StyleSheet _stylesheet;

        public StyleSheet Parse(string stylesheetText)
        {
            return Parse(new StylesheetReader(stylesheetText));
        }

        public StyleSheet Parse(Stream stylesheetStream)
        {
            return Parse(new StylesheetReader(stylesheetStream));
        }

        internal StyleSheet Parse(StylesheetReader reader)
        {
            Lexer = new Lexer(reader) { ErrorHandler = HandleLexerError };

            _stylesheet = new StyleSheet(Lexer);
            _stylesheet.BuildRules();

            return _stylesheet;
        }

        internal Lexer Lexer { get; private set; }

        internal void HandleLexerError(ParserError error, string message)
        {
            _stylesheet.Errors.Add(new StylesheetParseError(error, Lexer.Reader.Line, Lexer.Reader.Column, message));
        }
    }
}
