using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ExCSS.Model.TextBlocks;

namespace ExCSS
{
    internal class Lexer
    {
        private readonly StringBuilder _buffer;
        private readonly StylesheetReader _reader;

        internal Lexer(StylesheetReader reader) 
        {
            _buffer = new StringBuilder();
            _reader = reader;
            ErrorHandler = (pe, msg) => { };
        }

        internal Action<ParserError, string> ErrorHandler { get; set; }

        internal StylesheetReader Reader
        {
            get { return _reader; }
        }

        internal IEnumerable<Block> Tokens
        {
            get
            {
                while(true)
                {
                    var token = GetBlock(_reader.Current);

                    if (token == null)
                    {
                        yield break;
                    }

                    _reader.Advance();

                    yield return token;
                }
            }
        }

        private Block GetBlock(char current)
        {
            switch (current)
            {
                case Specification.LineFeed:
                case Specification.CarriageReturn:
                case Specification.Tab:
                case Specification.Space:
                    do
                    {
                        current = _reader.Next;
                    }
                    while (current.IsSpaceCharacter());

                    _reader.Back();

                    return SpecialCharacter.Whitespace;

                case Specification.DoubleQuote:
                    return DoubleQuoteString(_reader.Next);

                case Specification.Hash:
                    return HashStart(_reader.Next);

                case Specification.DollarSign:
                    current = _reader.Next;

                    return current == Specification.EqualSign 
                        ? MatchBlock.Suffix 
                        : Block.Delim(_reader.Previous);

                case Specification.SingleQuote:
                    return SingleQuoteString(_reader.Next);

                case '(':
                    return BracketBlock.OpenRound;

                case ')':
                    return BracketBlock.CloseRound;

                case Specification.Asterisk:
                    current = _reader.Next;

                    return current == Specification.EqualSign
                        ? MatchBlock.Substring 
                        : Block.Delim(_reader.Previous);

                case Specification.PlusSign:
                    {
                        var nextToken = _reader.Next;

                        if (nextToken == Specification.EndOfFile)
                        {
                            _reader.Back();
                        }
                        else
                        {
                            var nextDigit = _reader.Next;
                            _reader.Back(2);

                            if (nextToken.IsDigit() || (nextToken == Specification.Period && nextDigit.IsDigit()))
                            {
                                return NumberStart(current);
                            }
                        }
                        
                        return Block.Delim(current);
                    }

                case Specification.Comma:
                    return SpecialCharacter.Comma;

                case Specification.Period:
                    {
                        var periodToken = _reader.Next;

                        return periodToken.IsDigit() 
                            ? NumberStart(_reader.Previous) 
                            : Block.Delim(_reader.Previous);
                    }

                case Specification.MinusSign:
                    {
                        var token = _reader.Next;

                        if (token == Specification.EndOfFile)
                        {
                            _reader.Back();
                        }
                        else
                        {
                            var digiitToken = _reader.Next;
                            _reader.Back(2);

                            if (token.IsDigit() || (token == Specification.Period && digiitToken.IsDigit()))
                            {
                                return NumberStart(current);
                            }

                            if (token.IsNameStart())
                            {
                                return IdentStart(current);
                            }

                            if (token == Specification.ReverseSolidus && !digiitToken.IsLineBreak() && digiitToken != Specification.EndOfFile)
                             {
                                 return IdentStart(current);
                             }

                             if (token == Specification.MinusSign && digiitToken == Specification.GreaterThan)
                            {
                                _reader.Advance(2);
                                return CommentBlock.Close;
                            }
                        }
                        
                        return Block.Delim(current);
                    }

                case Specification.Solidus:
                    current = _reader.Next;

                    return current == Specification.Asterisk 
                        ? Comment(_reader.Next) 
                        : Block.Delim(_reader.Previous);

                case Specification.ReverseSolidus:
                    current = _reader.Next;

                    if (current.IsLineBreak() || current == Specification.EndOfFile)
                    {
                        ErrorHandler(current == Specification.EndOfFile 
                            ? ParserError.EndOfFile 
                            : ParserError.UnexpectedLineBreak,
                            "Unexpected line break or EOF.");

                        return Block.Delim(_reader.Previous);
                    }

                    return IdentStart(_reader.Previous);

                case Specification.Colon:
                    return SpecialCharacter.Colon;

                case Specification.Simicolon:
                    return SpecialCharacter.Semicolon;

                case Specification.LessThan:
                    current = _reader.Next;

                    if (current == Specification.Em)
                    {
                        current = _reader.Next;

                        if (current == Specification.MinusSign)
                        {
                            current = _reader.Next;

                            if (current == Specification.MinusSign)
                            {
                                return CommentBlock.Open;
                            }

                            current = _reader.Previous;
                        }

                        current = _reader.Previous;
                    }

                    return Block.Delim(_reader.Previous);

                case Specification.At:
                    return AtKeywordStart(_reader.Next);

                case '[':
                    return BracketBlock.OpenSquare;

                case ']':
                    return BracketBlock.CloseSquare;

                case Specification.Accent:
                    current = _reader.Next;

                    if (current == Specification.EqualSign)
                    {
                        return MatchBlock.Prefix;
                    }

                    return Block.Delim(_reader.Previous);

                case '{':
                    return BracketBlock.OpenCurly;

                case '}':
                    return BracketBlock.CloseCurly;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return NumberStart(current);

                case 'U':
                case 'u':
                    current = _reader.Next;

                    if (current == Specification.PlusSign)
                    {
                        current = _reader.Next;

                        if (current.IsHex() || current == Specification.QuestionMark)
                        {
                            return UnicodeRange(current);
                        }

                        current = _reader.Previous;
                    }

                    return IdentStart(_reader.Previous);

                case Specification.Pipe:
                    current = _reader.Next;

                    if (current == Specification.EqualSign)
                    {
                        return MatchBlock.Dash;
                    }
                    
                    if (current == Specification.Pipe)
                    {
                        return Block.Column;
                    }

                    return Block.Delim(_reader.Previous);

                case Specification.Tilde:
                    current = _reader.Next;

                    if (current == Specification.EqualSign)
                    {
                        return MatchBlock.Include;
                    }

                    return Block.Delim(_reader.Previous);

                case Specification.EndOfFile:
                    return null;

                case Specification.Em:
                    current = _reader.Next;

                    if (current == Specification.EqualSign)
                    {
                        return MatchBlock.Not;
                    }

                    return Block.Delim(_reader.Previous);

                default:
                    if (current.IsNameStart())
                    {
                        return IdentStart(current);
                    }

                    return Block.Delim(current);
            }
        }

        private Block DoubleQuoteString(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.DoubleQuote:
                    case Specification.EndOfFile:
                        return StringBlock.Plain(ClearBuffer());

                    case Specification.FormFeed:
                    case Specification.LineFeed:
                        ErrorHandler(ParserError.UnexpectedLineBreak, "Expected double quoted string to terminate before form feed or line feed.");
                        _reader.Back();
                        return StringBlock.Plain(ClearBuffer(), true);

                    case Specification.ReverseSolidus:
                        current = _reader.Next;

                        if (current.IsLineBreak())
                        {
                            _buffer.AppendLine();
                        }
                        else if (current != Specification.EndOfFile)
                        {
                            _buffer.Append(ConsumeEscape(current));
                        }
                        else
                        {
                            ErrorHandler(ParserError.EndOfFile, "Expected double quoted string to terminate before end of file.");
                            _reader.Back();
                            return StringBlock.Plain(ClearBuffer(), true);
                        }

                        break;

                    default:
                        _buffer.Append(current);
                        break;
                }

                current = _reader.Next;
            }
        }

        private Block SingleQuoteString(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.SingleQuote:
                    case Specification.EndOfFile:
                        return StringBlock.Plain(ClearBuffer());

                    case Specification.FormFeed:
                    case Specification.LineFeed:
                        ErrorHandler(ParserError.UnexpectedLineBreak, "Expected single quoted string to terminate before form feed or line feed.");
                        _reader.Back();
                        return (StringBlock.Plain(ClearBuffer(), true));

                    case Specification.ReverseSolidus:
                        current = _reader.Next;

                        if (current.IsLineBreak())
                        {
                            _buffer.AppendLine();
                        }
                        else if (current != Specification.EndOfFile)
                        {
                            _buffer.Append(ConsumeEscape(current));
                        }
                        else
                        {
                            ErrorHandler(ParserError.EndOfFile, "Expected single quoted string to terminate before end of file.");
                            _reader.Back();
                            return(StringBlock.Plain(ClearBuffer(), true));
                        }

                        break;

                    default:
                        _buffer.Append(current);
                        break;
                }

                current = _reader.Next;
            }
        }

        private Block HashStart(char current)
        {
            if (current.IsNameStart())
            {
                _buffer.Append(current);
                return HashRest(_reader.Next);
            }
            
            if (IsValidEscape(current))
            {
                current = _reader.Next;
                _buffer.Append(ConsumeEscape(current));
                return HashRest(_reader.Next);
            }
            
            if (current == Specification.ReverseSolidus)
            {
                ErrorHandler(ParserError.InvalidCharacter, "Invalid character after #.");
                _reader.Back();
                return Block.Delim(Specification.Hash);
            }
            
            _reader.Back();
            return Block.Delim(Specification.Hash);
        }

        private Block HashRest(char current)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else if (current == Specification.ReverseSolidus)
                {
                    ErrorHandler(ParserError.InvalidCharacter, "Invalid identifier after #.");
                    _reader.Back();
                    return SymbolBlock.Hash(ClearBuffer());
                }
                else
                {
                    _reader.Back();
                    return SymbolBlock.Hash(ClearBuffer());
                }

                current = _reader.Next;
            }
        }

        private Block Comment(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.Asterisk:
                        current = _reader.Next;
                       
                        if (current == Specification.Solidus)
                        {
                            return GetBlock(_reader.Next);
                        }
                       
                        break;

                    case Specification.EndOfFile:
                        return GetBlock(current);
                }

                current = _reader.Next;
            }
        }

        private Block AtKeywordStart(char current)
        {
            if (current == Specification.MinusSign)
            {
                current = _reader.Next;

                if (current.IsNameStart() || IsValidEscape(current))
                {
                    _buffer.Append(Specification.MinusSign);
                    return AtKeywordRest(current);
                }

                _reader.Back(2);
                return Block.Delim(Specification.At);
            }
            
            if (current.IsNameStart())
            {
                _buffer.Append(current);
                return AtKeywordRest(_reader.Next);
            }
            
            if (IsValidEscape(current))
            {
                current = _reader.Next;
                _buffer.Append(ConsumeEscape(current));
                return AtKeywordRest(_reader.Next);
            }
            
            _reader.Back();
            
            return Block.Delim(Specification.At);
        }

        private Block AtKeywordRest(char current)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else
                {
                    _reader.Back();
                    return SymbolBlock.At(ClearBuffer());
                }

                current = _reader.Next;
            }
        }

        private Block IdentStart(char current)
        {
            if (current == Specification.MinusSign)
            {
                current = _reader.Next;

                if (current.IsNameStart() || IsValidEscape(current))
                {
                    _buffer.Append(Specification.MinusSign);
                    return IdentRest(current);
                }

                _reader.Back();
                return Block.Delim(Specification.MinusSign);
            }
           
            if (current.IsNameStart())
            {
                _buffer.Append(current);
                return IdentRest(_reader.Next);
            }
            
            if (current == Specification.ReverseSolidus)
            {
                if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    _buffer.Append(ConsumeEscape(current));
                    return IdentRest(_reader.Next);
                }
            }

            return GetBlock(current);
        }

        private Block IdentRest(char current)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else if (current == '(')
                {
                    if (_buffer.ToString().Equals("url", StringComparison.OrdinalIgnoreCase))
                    {
                        _buffer.Clear();
                        return UrlStart(_reader.Next);
                    }

                    return SymbolBlock.Function(ClearBuffer());
                }
                else
                {
                    _reader.Back();
                    return SymbolBlock.Ident(ClearBuffer());
                }

                current = _reader.Next;
            }
        }

        private Block NumberStart(char current)
        {
            while (true)
            {
                if (current == Specification.PlusSign || current == Specification.MinusSign)
                {
                    _buffer.Append(current);
                    current = _reader.Next;

                    if (current == Specification.Period)
                    {
                        _buffer.Append(current);
                        _buffer.Append(_reader.Next);
                        return NumberFraction(_reader.Next);
                    }

                    _buffer.Append(current);
                    return NumberRest(_reader.Next);
                }
               
                if (current == Specification.Period)
                {
                    _buffer.Append(current);
                    _buffer.Append(_reader.Next);
                    return NumberFraction(_reader.Next);
                }
               
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                    return NumberRest(_reader.Next);
                }

                current = _reader.Next;
            }
        }

        private Block NumberRest(char current)
        {
            while (true)
            {
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                }
                else if (current.IsNameStart())
                {
                    var number = ClearBuffer();
                    _buffer.Append(current);
                    return Dimension(_reader.Next, number);
                }
                else if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    var number = ClearBuffer();
                    _buffer.Append(ConsumeEscape(current));
                    return Dimension(_reader.Next, number);
                }
                else
                {
                    break;
                }

                current = _reader.Next;
            }

            switch (current)
            {
                case Specification.Period:
                    current = _reader.Next;

                    if (current.IsDigit())
                    {
                        _buffer.Append(Specification.Period).Append(current);
                        return NumberFraction(_reader.Next);
                    }

                    _reader.Back();
                    return Block.Number(ClearBuffer());

                case '%':
                    return UnitBlock.Percentage(ClearBuffer());

                case 'e':
                case 'E':
                    return NumberExponential(current);

                case Specification.MinusSign:
                    return NumberDash(current);

                default:
                    _reader.Back();
                    return Block.Number(ClearBuffer());
            }
        }

        private Block NumberFraction(char current)
        {
            while (true)
            {
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                }
                else if (current.IsNameStart())
                {
                    var number = ClearBuffer();
                    _buffer.Append(current);
                    return Dimension(_reader.Next, number);
                }
                else if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    var number = ClearBuffer();
                    _buffer.Append(ConsumeEscape(current));
                    return Dimension(_reader.Next, number);
                }
                else
                {
                    break;
                }

                current = _reader.Next;
            }

            switch (current)
            {
                case 'e':
                case 'E':
                    return NumberExponential(current);

                case '%':
                    return UnitBlock.Percentage(ClearBuffer());

                case Specification.MinusSign:
                    return NumberDash(current);

                default:
                    _reader.Back();
                    return Block.Number(ClearBuffer());
            }
        }

        private Block Dimension(char current, string number)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else
                {
                    _reader.Back();
                    return UnitBlock.Dimension(number, ClearBuffer());
                }

                current = _reader.Next;
            }
        }

        private Block SciNotation(char current)
        {
            while (true)
            {
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                }
                else
                {
                    _reader.Back();
                    return Block.Number(ClearBuffer());
                }

                current = _reader.Next;
            }
        }

        private Block UrlStart(char current)
        {
            while (current.IsSpaceCharacter())
            {
                current = _reader.Next;
            }

            switch (current)
            {
                case Specification.EndOfFile:
                    ErrorHandler(ParserError.EndOfFile, "Expected URL to terminate before end of file.");
                    return StringBlock.Url(string.Empty, true);

                case Specification.DoubleQuote:
                    return DoubleQuoteUrl(_reader.Next);

                case Specification.SingleQuote:
                    return SingleQuoteUrl(_reader.Next);

                case ')':
                    return StringBlock.Url(string.Empty);

                default:
                    return UnquotedUrl(current);
            }
        }

        private Block DoubleQuoteUrl(char current)
        {
            while (true)
            {
                if (current.IsLineBreak())
                {
                    ErrorHandler(ParserError.UnexpectedLineBreak, "Expected URL to terminate before line break.");
                    return UrlBad(_reader.Next);
                }
               
                if (Specification.EndOfFile == current)
                {
                    return StringBlock.Url(ClearBuffer());
                }
                
                if (current == Specification.DoubleQuote)
                {
                    return UrlEnd(_reader.Next);
                }
                
                if (current == Specification.ReverseSolidus)
                {
                    current = _reader.Next;

                    if (current == Specification.EndOfFile)
                    {
                        _reader.Back(2);
                        ErrorHandler(ParserError.EndOfFile, "Expected URL to terminate before end of file.");
                        return StringBlock.Url(ClearBuffer(), true);
                    }

                    if (current.IsLineBreak())
                    {
                        _buffer.AppendLine();
                    }
                    else
                    {
                        _buffer.Append(ConsumeEscape(current));
                    }
                }
                else
                {
                    _buffer.Append(current);
                }

                current = _reader.Next;
            }
        }

        private Block SingleQuoteUrl(char current)
        {
            while (true)
            {
                if (current.IsLineBreak())
                {
                    ErrorHandler(ParserError.UnexpectedLineBreak, "Expected URL to terminate before line break.");
                    return UrlBad(_reader.Next);
                }
               
                if (Specification.EndOfFile == current)
                {
                    return StringBlock.Url(ClearBuffer());
                }
                
                if (current == Specification.SingleQuote)
                {
                    return UrlEnd(_reader.Next);
                }
                
                if (current == Specification.ReverseSolidus)
                {
                    current = _reader.Next;

                    if (current == Specification.EndOfFile)
                    {
                        _reader.Back(2);
                        ErrorHandler(ParserError.EndOfFile, "Expected URL to terminate before end of file.");
                        return StringBlock.Url(ClearBuffer(), true);
                    }

                    if (current.IsLineBreak())
                    {
                        _buffer.AppendLine();
                    }
                    else
                    {
                        _buffer.Append(ConsumeEscape(current));
                    }
                }
                else
                {
                    _buffer.Append(current);
                }

                current = _reader.Next;
            }
        }

        private Block UnquotedUrl(char current)
        {
            while (true)
            {
                if (current.IsSpaceCharacter())
                {
                    return UrlEnd(_reader.Next);
                }
                if (current == ')' || current == Specification.EndOfFile)
                {
                    return StringBlock.Url(ClearBuffer());
                }
                if (current == Specification.DoubleQuote || current == Specification.SingleQuote ||
                    current == '(' || current.IsNonPrintable())
                {
                    ErrorHandler(ParserError.InvalidCharacter, "Invalid quotation or open paren in URL.");
                    return UrlBad(_reader.Next);
                }
                if (current == Specification.ReverseSolidus)
                {
                    if (IsValidEscape(current))
                    {
                        current = _reader.Next;
                        _buffer.Append(ConsumeEscape(current));
                    }
                    else
                    {
                        ErrorHandler(ParserError.InvalidCharacter, "Invalid character in URL.");
                        return UrlBad(_reader.Next);
                    }
                }
                else
                {
                    _buffer.Append(current);
                }

                current = _reader.Next;
            }
        }

        private Block UrlEnd(char current)
        {
            while (true)
            {
                if (current == ')')
                {
                    return StringBlock.Url(ClearBuffer());
                }
                
                if (!current.IsSpaceCharacter())
                {
                    ErrorHandler(ParserError.InvalidCharacter, "Invalid character in URL.");
                    return UrlBad(current);
                }

                current = _reader.Next;
            }
        }

        private Block UrlBad(char current)
        {
            while (true)
            {
                if (current == Specification.EndOfFile)
                {
                    ErrorHandler(ParserError.EndOfFile, "Expected URL to terminate before end of file.");
                    return StringBlock.Url(ClearBuffer(), true);
                }
                
                if (current == ')')
                {
                    return StringBlock.Url(ClearBuffer(), true);
                }
                
                if (IsValidEscape(current))
                {
                    current = _reader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }

                current = _reader.Next;
            }
        }

        private Block UnicodeRange(char current)
        {
            for (var i = 0; i < 6; i++)
            {
                if (!current.IsHex())
                {
                    break;
                }

                _buffer.Append(current);
                current = _reader.Next;
            }

            if (_buffer.Length != 6)
            {
                for (var i = 0; i < 6 - _buffer.Length; i++)
                {
                    if (current != Specification.QuestionMark)
                    {
                        current = _reader.Previous;
                        break;
                    }

                    _buffer.Append(current);
                    current = _reader.Next;
                }

                var range = ClearBuffer();
                var start = range.Replace(Specification.QuestionMark, '0');
                var end = range.Replace(Specification.QuestionMark, 'F');
                return Block.Range(start, end);
            }
            
            if (current == Specification.MinusSign)
            {
                current = _reader.Next;

                if (current.IsHex())
                {
                    var start = _buffer.ToString();
                    _buffer.Clear();

                    for (var i = 0; i < 6; i++)
                    {
                        if (!current.IsHex())
                        {
                            current = _reader.Previous;
                            break;
                        }

                        _buffer.Append(current);
                        current = _reader.Next;
                    }

                    var end = ClearBuffer();
                    return Block.Range(start, end);
                }
                _reader.Back(2);

                return Block.Range(ClearBuffer(), null);
                
            }
           
            _reader.Back();

            return Block.Range(ClearBuffer(), null);
        }
 
        private Block NumberExponential(char current)
        {
            current = _reader.Next;

            if (current.IsDigit())
            {
                _buffer.Append('e').Append(current);
                return SciNotation(_reader.Next);
            }
            
            if (current == Specification.PlusSign || current == Specification.MinusSign)
            {
                var sign = current;
                current = _reader.Next;

                if (current.IsDigit())
                {
                    _buffer.Append('e').Append(sign).Append(current);
                    return SciNotation(_reader.Next);
                }

                _reader.Back();
            }

            current = _reader.Previous;
            var number = ClearBuffer();

            _buffer.Append(current);

            return Dimension(_reader.Next, number);
        }

        private Block NumberDash(char current)
        {
            current = _reader.Next;

            if (current.IsNameStart())
            {
                var number = ClearBuffer();
                _buffer.Append(Specification.MinusSign).Append(current);
               
                return Dimension(_reader.Next, number);
            }
            
            if (IsValidEscape(current))
            {
                current = _reader.Next;
                var number = ClearBuffer();
                _buffer.Append(Specification.MinusSign).Append(ConsumeEscape(current));
               
                return Dimension(_reader.Next, number);
            }
            _reader.Back(2);

            return Block.Number(ClearBuffer());
        }

        private string ClearBuffer()
        {
            var val = _buffer.ToString();
            _buffer.Clear();

            return val;
        }

        private string ConsumeEscape(char current)
        {
            if (current.IsHex())
            {
                var escape = new List<char>();

                for (var i = 0; i < 6; i++)
                {
                    escape.Add(current);
                    current = _reader.Next;

                    if (!current.IsHex())
                    {
                        break;
                    }
                }

                current = _reader.Previous;
                var code = int.Parse(new string(escape.ToArray()), NumberStyles.HexNumber);
                
                return char.ConvertFromUtf32(code);
            }

            return current.ToString();
        }

        private bool IsValidEscape(char current)
        {
            if (current != Specification.ReverseSolidus)
            {
                return false;
            }

            current = _reader.Next;
            _reader.Back();

            if (current == Specification.EndOfFile)

            {
                return false;
            }
            
            return !current.IsLineBreak();
        }
    }
}
