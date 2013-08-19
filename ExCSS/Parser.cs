using System.IO;
using ExCSS.Model;

namespace ExCSS
{
    public class Parser
    {
        private StyleSheetContext _stylesheetContext;

        public StyleSheetContext Parse(string stylesheetText)
        {
            return Parse(new StylesheetStreamReader(stylesheetText));
        }

        public StyleSheetContext Parse(Stream stylesheetStream)
        {
            return Parse(new StylesheetStreamReader(stylesheetStream));
        }

        internal StyleSheetContext Parse(StylesheetStreamReader reader)
        {
            Lexer = new Lexer(reader);
            
            //_lexer.ErrorOccurred += (s, ev) =>
            //{
            //    if (ErrorOccurred != null)
            //        ErrorOccurred(this, ev);
            //};

            _stylesheetContext = new StyleSheetContext(Lexer);
            _stylesheetContext.BuildRules();

            return _stylesheetContext;
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
