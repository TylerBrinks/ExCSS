using System.IO;
using ExCSS.Model;

namespace ExCSS
{
    public class Parser
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
            Lexer = new Lexer(reader);
            
            //_lexer.ErrorOccurred += (s, ev) =>
            //{
            //    if (ErrorOccurred != null)
            //        ErrorOccurred(this, ev);
            //};

            _stylesheet = new StyleSheet(Lexer);
            _stylesheet.BuildRules();

            return _stylesheet;
        }

        internal Lexer Lexer { get; private set; }

        //void RaiseErrorOccurred(ErrorCode code)
        //{
        //    if (ErrorOccurred != null)
        //    {
        //        var pck = new ParseErrorEventArgs((int)code, Errors.GetError(code));
        //        pck.Line = _lexer.Reader.Line;
        //        pck.Column = _lexer.Reader.Column;
        //        ErrorOccurred(this, pck);
        //    }
        //}
    }
}
