using System.IO;
using ExCSS.Model;

namespace ExCSS
{
    public class Parser
    {
        private readonly Lexer _lexer;
        private StyleSheetContext _stylesheetContext;

        public Parser(string reader) : this(new StylesheetStreamReader(reader))
        {
        }

        public Parser(Stream reader) : this(new StylesheetStreamReader(reader))
        {
        }

        internal Parser(StylesheetStreamReader reader)
        {
            _lexer = new Lexer(reader);

            //_lexer.ErrorOccurred += (s, ev) =>
            //{
            //    if (ErrorOccurred != null)
            //        ErrorOccurred(this, ev);
            //};
        }

        public StyleSheetContext Parse()
        {
            _stylesheetContext = new StyleSheetContext(_lexer);
            _stylesheetContext.BuildRules();

            return _stylesheetContext;
        }

        internal Lexer Lexer { get { return _lexer; } }

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
