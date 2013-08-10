using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace ExCSS.Model
{
    sealed class SourceManager
    {
        #region Members

        Int32 _column;
        Int32 _row;
        Int32 _insertion;
        Stack<Int32> _collengths;
        Char _current;
        TextReader _reader;
        StringBuilder _buffer;
        bool _ended;
        bool _lwcr;
        Encoding _encoding;

        #endregion

        #region ctor

        /// <summary>
        /// Prepares everything.
        /// </summary>
        SourceManager()
        {
            _encoding = HtmlEncoding.Suggest(CultureInfo.CurrentUICulture.Name);//LocalSettings.Language);
            _buffer = new StringBuilder();
            _collengths = new Stack<int>();
            _column = 1;
            _row = 1;
        }

        /// <summary>
        /// Constructs a new instance of the source code manager.
        /// </summary>
        /// <param name="source">The source code string to manage.</param>
        public SourceManager(string source)
            : this()
        {
            _reader = new StringReader(source);
            ReadCurrent();
        }

        /// <summary>
        /// Constructs a new instance of the source code manager.
        /// </summary>
        /// <param name="stream">The source code stream to manage.</param>
        public SourceManager(Stream stream)
            : this()
        {
            _reader = new StreamReader(stream, true);
            ReadCurrent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the position is at the moment at the beginning.
        /// </summary>
        public bool IsBeginning
        {
            get { return _insertion < 2; }
        }

        /// <summary>
        /// Gets or sets the encoding to use.
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set
            {
                _encoding = value;

                if (_reader is StreamReader)
                {
                    var chars = _buffer.Length;
                    var stream = ((StreamReader)_reader).BaseStream;
                    _insertion = 0;
                    stream.Position = 0;
                    _buffer.Clear();
                    _reader = new StreamReader(stream, value);
                    Advance(chars);
                }
            }
        }

        /// <summary>
        /// Gets or sets the insertion point.
        /// </summary>
        public Int32 InsertionPoint
        {
            get { return _insertion; }
            set
            {
                if (value >= 0 && value <= _buffer.Length)
                {
                    var delta = _insertion - value;

                    if (delta > 0)
                    {
                        while (_insertion != value)
                            BackUnsafe();
                    }
                    else if (delta < 0)
                    {
                        while (_insertion != value)
                            AdvanceUnsafe();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current line within the source code.
        /// </summary>
        public Int32 Line
        {
            get { return _row; }
        }

        /// <summary>
        /// Gets the current column within the source code.
        /// </summary>
        public Int32 Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Gets the status of reading the source code, are we beyond the stream?
        /// </summary>
        public bool IsEnded
        {
            get { return _ended; }
        }

        /// <summary>
        /// Gets the status of reading the source code, is the EOF currently given?
        /// </summary>
        public bool IsEnding
        {
            get { return _current == Specification.EOF; }
        }

        /// <summary>
        /// Gets the current character.
        /// </summary>
        public Char Current
        {
            get { return _current; }
        }

        /// <summary>
        /// Gets the next character (by advancing and returning the current character).
        /// </summary>
        [DebuggerHidden]
        public Char Next
        {
            get { Advance(); return _current; }
        }

        /// <summary>
        /// Gets the previous character (by rewinding and returning the current character).
        /// </summary>
        [DebuggerHidden]
        public Char Previous
        {
            get { Back(); return _current; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the insertion point to the end of the buffer.
        /// </summary>
        //[DebuggerStepThrough]
        public void ResetInsertionPoint()
        {
            InsertionPoint = _buffer.Length;
        }

        /// <summary>
        /// Advances one character in the source code.
        /// </summary>
        /// <returns>The current source manager.</returns>
        //[DebuggerStepThrough]
        public void Advance()
        {
            if (!IsEnding)
                AdvanceUnsafe();
            else if (!_ended)
                _ended = true;
        }

        /// <summary>
        /// Advances n characters in the source code.
        /// </summary>
        /// <param name="n">The number of characters to advance.</param>
        //[DebuggerStepThrough]
        public void Advance(Int32 n)
        {
            while (n-- > 0 && !IsEnding)
                AdvanceUnsafe();
        }

        /// <summary>
        /// Moves back one character in the source code.
        /// </summary>
        //[DebuggerStepThrough]
        public void Back()
        {
            _ended = false;

            if (!IsBeginning)
                BackUnsafe();
        }

        /// <summary>
        /// Moves back n characters in the source code.
        /// </summary>
        /// <param name="n">The number of characters to rewind.</param>
        //[DebuggerStepThrough]
        public void Back(Int32 n)
        {
            _ended = false;

            while (n-- > 0 && !IsBeginning)
                BackUnsafe();
        }

        /// <summary>
        /// Looks if the current character / next characters match a certain string.
        /// </summary>
        /// <param name="s">The string to compare to.</param>
        /// <param name="ignoreCase">Optional flag to unignore the case sensitivity.</param>
        /// <returns>The status of the check.</returns>
        //[DebuggerStepThrough]
        public bool ContinuesWith(string s, bool ignoreCase = true)
        {
            for (var index = 0; index < s.Length; index++)
            {
                var chr = _current;

                if (ignoreCase && chr.IsUppercaseAscii())
                    chr = chr.ToLower();

                if (s[index] != chr)
                {
                    Back(index);
                    return false;
                }

                Advance();
            }

            Back(s.Length);
            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Reads the current character (from the stream or
        /// from the buffer).
        /// </summary>
        //[DebuggerStepThrough]
        void ReadCurrent()
        {
            if (_insertion < _buffer.Length)
            {
                _current = _buffer[_insertion];
                _insertion++;
                return;
            }

            var tmp = _reader.Read();
            _current = tmp == -1 ? Specification.EOF : (Char)tmp;

            if (_current == Specification.CR)
            {
                _current = Specification.LF;
                _lwcr = true;
            }
            else if (_lwcr)
            {
                _lwcr = false;

                if (_current == Specification.LF)
                {
                    ReadCurrent();
                    return;
                }
            }

            _buffer.Append(_current);
            _insertion++;
        }

        /// <summary>
        /// Just advances one character without checking
        /// if the end is already reached.
        /// </summary>
        //[DebuggerStepThrough]
        void AdvanceUnsafe()
        {
            if (_current.IsLineBreak())
            {
                _collengths.Push(_column);
                _column = 1;
                _row++;
            }
            else
                _column++;

            ReadCurrent();
        }

        /// <summary>
        /// Just goes back one character without checking
        /// if the beginning is already reached.
        /// </summary>
        //[DebuggerStepThrough]
        void BackUnsafe()
        {
            _insertion--;

            if (_insertion == 0)
            {
                _column = 0;
                _current = Specification.NULL;
                return;
            }

            _current = _buffer[_insertion - 1];

            if (_current.IsLineBreak())
            {
                _column = _collengths.Count != 0 ? _collengths.Pop() : 1;
                _row--;
            }
            else
                _column--;
        }

        #endregion
    }
    
    ////[DebuggerStepThrough]
    sealed class Lexer : BaseTokenizer
    {
        #region ctor

        public Lexer(SourceManager source)
            : base(source)
        {
            stringBuffer = new StringBuilder();
            src = source;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying stream.
        /// </summary>
        public SourceManager Stream
        {
            get { return src; }
        }

        ///// <summary>
        ///// Gets the iterator for the tokens.
        ///// </summary>
        //public IEnumerator<Block> Iterator
        //{
        //    get { return Tokens.GetEnumerator(); }
        //}

        /// <summary>
        /// Gets the token enumerable.
        /// </summary>
        public IEnumerable<Block> Tokens
        {
            get
            {
                while(true)
                {
                    Block token = Data(src.Current);

                    if (token == null)
                    {
                        yield break;
                    }

                    src.Advance();
                    yield return token;
                }
            }
        }

        #endregion

        #region States

        /// <summary>
        /// 4.4.1. Value state
        /// </summary>
        Block Data(Char current)
        {
            switch (current)
            {
                case Specification.LF:
                case Specification.CR:
                case Specification.TAB:
                case Specification.SPACE:
                    do { current = src.Next; }
                    while (current.IsSpaceCharacter());
                    src.Back();
                    return SpecialCharacter.Whitespace;

                case Specification.DQ:
                    return StringDQ(src.Next);

                case Specification.NUM:
                    return HashStart(src.Next);

                case Specification.DOLLAR:
                    current = src.Next;

                    if (current == Specification.EQ)
                        return MatchBlock.Suffix;

                    return Block.Delim(src.Previous);

                case Specification.SQ:
                    return StringSQ(src.Next);

                case '(':
                    return BracketBlock.OpenRound;

                case ')':
                    return BracketBlock.CloseRound;

                case Specification.ASTERISK:
                    current = src.Next;

                    if (current == Specification.EQ)
                        return MatchBlock.Substring;

                    return Block.Delim(src.Previous);

                case Specification.PLUS:
                    {
                        var c1 = src.Next;

                        if (c1 == Specification.EOF)
                        {
                            src.Back();
                        }
                        else
                        {
                            var c2 = src.Next;
                            src.Back(2);

                            if (c1.IsDigit() || (c1 == Specification.DOT && c2.IsDigit()))
                                return NumberStart(current);
                        }
                        
                        return Block.Delim(current);
                    }

                case Specification.COMMA:
                    return SpecialCharacter.Comma;

                case Specification.DOT:
                    {
                        var c = src.Next;

                        if (c.IsDigit())
                            return NumberStart(src.Previous);
                        
                        return Block.Delim(src.Previous);
                    }

                case Specification.MINUS:
                    {
                        var c1 = src.Next;

                        if (c1 == Specification.EOF)
                        {
                            src.Back();
                        }
                        else
                        {
                            var c2 = src.Next;
                            src.Back(2);

                            if (c1.IsDigit() || (c1 == Specification.DOT && c2.IsDigit()))
                                return NumberStart(current);
                            else if (c1.IsNameStart())
                                return IdentStart(current);
                            else if (c1 == Specification.RSOLIDUS && !c2.IsLineBreak() && c2 != Specification.EOF)
                                return IdentStart(current);
                            else if (c1 == Specification.MINUS && c2 == Specification.GT)
                            {
                                src.Advance(2);
                                return CommentBlock.Close;
                            }
                        }
                        
                        return Block.Delim(current);
                    }

                case Specification.SOLIDUS:
                    current = src.Next;

                    if (current == Specification.ASTERISK)
                        return Comment(src.Next);
                        
                    return Block.Delim(src.Previous);

                case Specification.RSOLIDUS:
                    current = src.Next;

                    if (current.IsLineBreak() || current == Specification.EOF)
                    {
                        //RaiseErrorOccurred(current == Specification.EOF ? ErrorCode.EOF : ErrorCode.LineBreakUnexpected);
                        return Block.Delim(src.Previous);
                    }

                    return IdentStart(src.Previous);

                case Specification.COLON:
                    return SpecialCharacter.Colon;

                case Specification.SC:
                    return SpecialCharacter.Semicolon;

                case Specification.LT:
                    current = src.Next;

                    if (current == Specification.EM)
                    {
                        current = src.Next;

                        if (current == Specification.MINUS)
                        {
                            current = src.Next;

                            if (current == Specification.MINUS)
                                return CommentBlock.Open;

                            current = src.Previous;
                        }

                        current = src.Previous;
                    }

                    return Block.Delim(src.Previous);

                case Specification.AT:
                    return AtKeywordStart(src.Next);

                case '[':
                    return BracketBlock.OpenSquare;

                case ']':
                    return BracketBlock.CloseSquare;

                case Specification.ACCENT:
                    current = src.Next;

                    if (current == Specification.EQ)
                        return MatchBlock.Prefix;

                    return Block.Delim(src.Previous);

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
                    current = src.Next;

                    if (current == Specification.PLUS)
                    {
                        current = src.Next;

                        if (current.IsHex() || current == Specification.QM)
                            return UnicodeRange(current);

                        current = src.Previous;
                    }

                    return IdentStart(src.Previous);

                case Specification.PIPE:
                    current = src.Next;

                    if (current == Specification.EQ)
                        return MatchBlock.Dash;
                    else if (current == Specification.PIPE)
                        return Block.Column;

                    return Block.Delim(src.Previous);

                case Specification.TILDE:
                    current = src.Next;

                    if (current == Specification.EQ)
                        return MatchBlock.Include;

                    return Block.Delim(src.Previous);

                case Specification.EOF:
                    return null;

                case Specification.EM:
                    current = src.Next;

                    if (current == Specification.EQ)
                        return MatchBlock.Not;

                    return Block.Delim(src.Previous);

                default:
                    if (current.IsNameStart())
                        return IdentStart(current);

                    return Block.Delim(current);
            }
        }

        /// <summary>
        /// 4.4.2. Double quoted string state
        /// </summary>
        Block StringDQ(Char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.DQ:
                    case Specification.EOF:
                        return StringBlock.Plain(FlushBuffer());

                    case Specification.FF:
                    case Specification.LF:
                        //RaiseErrorOccurred(ErrorCode.LineBreakUnexpected);
                        src.Back();
                        return StringBlock.Plain(FlushBuffer(), true);

                    case Specification.RSOLIDUS:
                        current = src.Next;

                        if (current.IsLineBreak())
                            stringBuffer.AppendLine();
                        else if (current != Specification.EOF)
                            stringBuffer.Append(ConsumeEscape(current));
                        else
                        {
                            //RaiseErrorOccurred(ErrorCode.EOF);
                            src.Back();
                            return StringBlock.Plain(FlushBuffer(), true);
                        }

                        break;

                    default:
                        stringBuffer.Append(current);
                        break;
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.3. Single quoted string state
        /// </summary>
        Block StringSQ(Char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.SQ:
                    case Specification.EOF:
                        return StringBlock.Plain(FlushBuffer());

                    case Specification.FF:
                    case Specification.LF:
                        //RaiseErrorOccurred(ErrorCode.LineBreakUnexpected);
                        src.Back();
                        return (StringBlock.Plain(FlushBuffer(), true));

                    case Specification.RSOLIDUS:
                        current = src.Next;

                        if (current.IsLineBreak())
                            stringBuffer.AppendLine();
                        else if (current != Specification.EOF)
                            stringBuffer.Append(ConsumeEscape(current));
                        else
                        {
                            //RaiseErrorOccurred(ErrorCode.EOF);
                            src.Back();
                            return(StringBlock.Plain(FlushBuffer(), true));
                        }

                        break;

                    default:
                        stringBuffer.Append(current);
                        break;
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.4. Hash state
        /// </summary>
        Block HashStart(Char current)
        {
            if (current.IsNameStart())
            {
                stringBuffer.Append(current);
                return HashRest(src.Next);
            }
            else if (IsValidEscape(current))
            {
                current = src.Next;
                stringBuffer.Append(ConsumeEscape(current));
                return HashRest(src.Next);
            }
            else if (current == Specification.RSOLIDUS)
            {
                //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                src.Back();
                return Block.Delim(Specification.NUM);
            }
            else
            {
                src.Back();
                return Block.Delim(Specification.NUM);
            }
        }

        /// <summary>
        /// 4.4.5. Hash-rest state
        /// </summary>
        Block HashRest(Char current)
        {
            while (true)
            {
                if (current.IsName())
                    stringBuffer.Append(current);
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    stringBuffer.Append(ConsumeEscape(current));
                }
                else if (current == Specification.RSOLIDUS)
                {
                    //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                    src.Back();
                    return SymbolBlock.Hash(FlushBuffer());
                }
                else
                {
                    src.Back();
                    return SymbolBlock.Hash(FlushBuffer());
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.6. Comment state
        /// </summary>
        Block Comment(Char current)
        {
            while (true)
            {
                if (current == Specification.ASTERISK)
                {
                    current = src.Next;

                    if (current == Specification.SOLIDUS)
                        return Data(src.Next);
                }
                else if (current == Specification.EOF)
                {
                    //RaiseErrorOccurred(ErrorCode.EOF);
                    return Data(current);
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.7. At-keyword state
        /// </summary>
        Block AtKeywordStart(Char current)
        {
            if (current == Specification.MINUS)
            {
                current = src.Next;

                if (current.IsNameStart() || IsValidEscape(current))
                {
                    stringBuffer.Append(Specification.MINUS);
                    return AtKeywordRest(current);
                }

                src.Back(2);
                return Block.Delim(Specification.AT);
            }
            else if (current.IsNameStart())
            {
                stringBuffer.Append(current);
                return AtKeywordRest(src.Next);
            }
            else if (IsValidEscape(current))
            {
                current = src.Next;
                stringBuffer.Append(ConsumeEscape(current));
                return AtKeywordRest(src.Next);
            }
            else
            {
                src.Back();
                return Block.Delim(Specification.AT);
            }
        }

        /// <summary>
        /// 4.4.8. At-keyword-rest state
        /// </summary>
        Block AtKeywordRest(Char current)
        {
            while (true)
            {
                if (current.IsName())
                    stringBuffer.Append(current);
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    stringBuffer.Append(ConsumeEscape(current));
                }
                else
                {
                    src.Back();
                    return SymbolBlock.At(FlushBuffer());
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.9. Ident state
        /// </summary>
        Block IdentStart(Char current)
        {
            if (current == Specification.MINUS)
            {
                current = src.Next;

                if (current.IsNameStart() || IsValidEscape(current))
                {
                    stringBuffer.Append(Specification.MINUS);
                    return IdentRest(current);
                }

                src.Back();
                return Block.Delim(Specification.MINUS);
            }
            else if (current.IsNameStart())
            {
                stringBuffer.Append(current);
                return IdentRest(src.Next);
            }
            else if (current == Specification.RSOLIDUS)
            {
                if (IsValidEscape(current))
                {
                    current = src.Next;
                    stringBuffer.Append(ConsumeEscape(current));
                    return IdentRest(src.Next);
                }
            }

            return Data(current);
        }

        /// <summary>
        /// 4.4.10. Ident-rest state
        /// </summary>
        Block IdentRest(Char current)
        {
            while (true)
            {
                if (current.IsName())
                    stringBuffer.Append(current);
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    stringBuffer.Append(ConsumeEscape(current));
                }
                else if (current == '(')
                {
                    if (stringBuffer.ToString().Equals("url", StringComparison.OrdinalIgnoreCase))
                    {
                        stringBuffer.Clear();
                        return UrlStart(src.Next);
                    }

                    return SymbolBlock.Function(FlushBuffer());
                }
                //false could be replaced with a transform whitespace flag, which is set to true if in SVG transform mode.
                //else if (false && Specification.IsSpaceCharacter(current))
                //    InstantSwitch(TransformFunctionWhitespace);
                else
                {
                    src.Back();
                    return SymbolBlock.Ident(FlushBuffer());
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.11. Transform-function-whitespace state
        /// </summary>
        Block TransformFunctionWhitespace(Char current)
        {
            while (true)
            {
                current = src.Next;

                if (current == '(')
                {
                    src.Back();
                    return SymbolBlock.Function(FlushBuffer());
                }
                else if (!current.IsSpaceCharacter())
                {
                    src.Back(2);
                    return SymbolBlock.Ident(FlushBuffer());
                }
            }
        }

        /// <summary>
        /// 4.4.12. Number state
        /// </summary>
        Block NumberStart(Char current)
        {
            while (true)
            {
                if (current == Specification.PLUS || current == Specification.MINUS)
                {
                    stringBuffer.Append(current);
                    current = src.Next;

                    if (current == Specification.DOT)
                    {
                        stringBuffer.Append(current);
                        stringBuffer.Append(src.Next);
                        return NumberFraction(src.Next);
                    }

                    stringBuffer.Append(current);
                    return NumberRest(src.Next);
                }
                else if (current == Specification.DOT)
                {
                    stringBuffer.Append(current);
                    stringBuffer.Append(src.Next);
                    return NumberFraction(src.Next);
                }
                else if (current.IsDigit())
                {
                    stringBuffer.Append(current);
                    return NumberRest(src.Next);
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.13. Number-rest state
        /// </summary>
        Block NumberRest(Char current)
        {
            while (true)
            {
                if (current.IsDigit())
                    stringBuffer.Append(current);
                else if (current.IsNameStart())
                {
                    var number = FlushBuffer();
                    stringBuffer.Append(current);
                    return Dimension(src.Next, number);
                }
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    var number = FlushBuffer();
                    stringBuffer.Append(ConsumeEscape(current));
                    return Dimension(src.Next, number);
                }
                else
                    break;

                current = src.Next;
            }

            switch (current)
            {
                case Specification.DOT:
                    current = src.Next;

                    if (current.IsDigit())
                    {
                        stringBuffer.Append(Specification.DOT).Append(current);
                        return NumberFraction(src.Next);
                    }

                    src.Back();
                    return Block.Number(FlushBuffer());

                case '%':
                    return UnitBlock.Percentage(FlushBuffer());

                case 'e':
                case 'E':
                    return NumberExponential(current);

                case Specification.MINUS:
                    return NumberDash(current);

                default:
                    src.Back();
                    return Block.Number(FlushBuffer());
            }
        }

        /// <summary>
        /// 4.4.14. Number-fraction state
        /// </summary>
        Block NumberFraction(Char current)
        {
            while (true)
            {
                if (current.IsDigit())
                    stringBuffer.Append(current);
                else if (current.IsNameStart())
                {
                    var number = FlushBuffer();
                    stringBuffer.Append(current);
                    return Dimension(src.Next, number);
                }
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    var number = FlushBuffer();
                    stringBuffer.Append(ConsumeEscape(current));
                    return Dimension(src.Next, number);
                }
                else
                    break;

                current = src.Next;
            }

            switch (current)
            {
                case 'e':
                case 'E':
                    return NumberExponential(current);

                case '%':
                    return UnitBlock.Percentage(FlushBuffer());

                case Specification.MINUS:
                    return NumberDash(current);

                default:
                    src.Back();
                    return Block.Number(FlushBuffer());
            }
        }

        /// <summary>
        /// 4.4.15. Dimension state
        /// </summary>
        Block Dimension(Char current, string number)
        {
            while (true)
            {
                if (current.IsName())
                    stringBuffer.Append(current);
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    stringBuffer.Append(ConsumeEscape(current));
                }
                else
                {
                    src.Back();
                    return UnitBlock.Dimension(number, FlushBuffer());
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.16. SciNotation state
        /// </summary>
        Block SciNotation(Char current)
        {
            while (true)
            {
                if (current.IsDigit())
                    stringBuffer.Append(current);
                else
                {
                    src.Back();
                    return Block.Number(FlushBuffer());
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.17. URL state
        /// </summary>
        Block UrlStart(Char current)
        {
            while (current.IsSpaceCharacter())
                current = src.Next;

            switch (current)
            {
                case Specification.EOF:
                    //RaiseErrorOccurred(ErrorCode.EOF);
                    return StringBlock.Url(String.Empty, true);

                case Specification.DQ:
                    return UrlDQ(src.Next);

                case Specification.SQ:
                    return UrlSQ(src.Next);

                case ')':
                    return StringBlock.Url(String.Empty, false);

                default:
                    return UrlUQ(current);
            }
        }

        /// <summary>
        /// 4.4.18. URL-double-quoted state
        /// </summary>
        Block UrlDQ(Char current)
        {
            while (true)
            {
                if (current.IsLineBreak())
                {
                    //RaiseErrorOccurred(ErrorCode.LineBreakUnexpected);
                    return UrlBad(src.Next);
                }
                else if (Specification.EOF == current)
                {
                    return StringBlock.Url(FlushBuffer());
                }
                else if (current == Specification.DQ)
                {
                    return UrlEnd(src.Next);
                }
                else if (current == Specification.RSOLIDUS)
                {
                    current = src.Next;

                    if (current == Specification.EOF)
                    {
                        src.Back(2);
                        //RaiseErrorOccurred(ErrorCode.EOF);
                        return StringBlock.Url(FlushBuffer(), true);
                    }
                    else if (current.IsLineBreak())
                        stringBuffer.AppendLine();
                    else
                        stringBuffer.Append(ConsumeEscape(current));
                }
                else
                    stringBuffer.Append(current);

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.19. URL-single-quoted state
        /// </summary>
        Block UrlSQ(Char current)
        {
            while (true)
            {
                if (current.IsLineBreak())
                {
                    //RaiseErrorOccurred(ErrorCode.LineBreakUnexpected);
                    return UrlBad(src.Next);
                }
                else if (Specification.EOF == current)
                {
                    return StringBlock.Url(FlushBuffer());
                }
                else if (current == Specification.SQ)
                {
                    return UrlEnd(src.Next);
                }
                else if (current == Specification.RSOLIDUS)
                {
                    current = src.Next;

                    if (current == Specification.EOF)
                    {
                        src.Back(2);
                        //RaiseErrorOccurred(ErrorCode.EOF);
                        return StringBlock.Url(FlushBuffer(), true);
                    }
                    else if (current.IsLineBreak())
                        stringBuffer.AppendLine();
                    else
                        stringBuffer.Append(ConsumeEscape(current));
                }
                else
                    stringBuffer.Append(current);

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.21. URL-unquoted state
        /// </summary>
        Block UrlUQ(Char current)
        {
            while (true)
            {
                if (current.IsSpaceCharacter())
                {
                    return UrlEnd(src.Next);
                }
                else if (current == ')' || current == Specification.EOF)
                {
                    return StringBlock.Url(FlushBuffer());
                }
                else if (current == Specification.DQ || current == Specification.SQ || current == '(' || current.IsNonPrintable())
                {
                    //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                    return UrlBad(src.Next);
                }
                else if (current == Specification.RSOLIDUS)
                {
                    if (IsValidEscape(current))
                    {
                        current = src.Next;
                        stringBuffer.Append(ConsumeEscape(current));
                    }
                    else
                    {
                        //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                        return UrlBad(src.Next);
                    }
                }
                else
                    stringBuffer.Append(current);

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.20. URL-end state
        /// </summary>
        Block UrlEnd(Char current)
        {
            while (true)
            {
                if (current == ')')
                    return StringBlock.Url(FlushBuffer());
                else if (!current.IsSpaceCharacter())
                {
                    //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                    return UrlBad(current);
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.22. Bad URL state
        /// </summary>
        Block UrlBad(Char current)
        {
            while (true)
            {
                if (current == Specification.EOF)
                {
                    //RaiseErrorOccurred(ErrorCode.EOF);
                    return StringBlock.Url(FlushBuffer(), true);
                }
                else if (current == ')')
                {
                    return StringBlock.Url(FlushBuffer(), true);
                }
                else if (IsValidEscape(current))
                {
                    current = src.Next;
                    stringBuffer.Append(ConsumeEscape(current));
                }

                current = src.Next;
            }
        }

        /// <summary>
        /// 4.4.23. Unicode-range State
        /// </summary>
        Block UnicodeRange(Char current)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!current.IsHex())
                    break;

                stringBuffer.Append(current);
                current = src.Next;
            }

            if (stringBuffer.Length != 6)
            {
                for (int i = 0; i < 6 - stringBuffer.Length; i++)
                {
                    if (current != Specification.QM)
                    {
                        current = src.Previous;
                        break;
                    }

                    stringBuffer.Append(current);
                    current = src.Next;
                }

                var range = FlushBuffer();
                var start = range.Replace(Specification.QM, '0');
                var end = range.Replace(Specification.QM, 'F');
                return Block.Range(start, end);
            }
            else if (current == Specification.MINUS)
            {
                current = src.Next;

                if (current.IsHex())
                {
                    var start = stringBuffer.ToString();
                    stringBuffer.Clear();

                    for (int i = 0; i < 6; i++)
                    {
                        if (!current.IsHex())
                        {
                            current = src.Previous;
                            break;
                        }

                        stringBuffer.Append(current);
                        current = src.Next;
                    }

                    var end = FlushBuffer();
                    return Block.Range(start, end);
                }
                else
                {
                    src.Back(2);
                    return Block.Range(FlushBuffer(), null);
                }
            }
            else
            {
                src.Back();
                return Block.Range(FlushBuffer(), null);
            }
        }

        #endregion

        #region Helpers

        string FlushBuffer()
        {
            var tmp = stringBuffer.ToString();
            stringBuffer.Clear();
            return tmp;
        }

        /// <summary>
        /// Substate of several Number states.
        /// </summary>
        Block NumberExponential(Char current)
        {
            current = src.Next;

            if (current.IsDigit())
            {
                stringBuffer.Append('e').Append(current);
                return SciNotation(src.Next);
            }
            else if (current == Specification.PLUS || current == Specification.MINUS)
            {
                var op = current;
                current = src.Next;

                if (current.IsDigit())
                {
                    stringBuffer.Append('e').Append(op).Append(current);
                    return SciNotation(src.Next);
                }

                src.Back();
            }

            current = src.Previous;
            var number = FlushBuffer();
            stringBuffer.Append(current);
            return Dimension(src.Next, number);
        }

        /// <summary>
        /// Substate of several Number states.
        /// </summary>
        Block NumberDash(Char current)
        {
            current = src.Next;

            if (current.IsNameStart())
            {
                var number = FlushBuffer();
                stringBuffer.Append(Specification.MINUS).Append(current);
                return Dimension(src.Next, number);
            }
            else if (IsValidEscape(current))
            {
                current = src.Next;
                var number = FlushBuffer();
                stringBuffer.Append(Specification.MINUS).Append(ConsumeEscape(current));
                return Dimension(src.Next, number);
            }
            else
            {
                src.Back(2);
                return Block.Number(FlushBuffer());
            }
        }

        /// <summary>
        /// Consumes an escaped character AFTER the solidus has already been
        /// consumed.
        /// </summary>
        /// <returns>The escaped character.</returns>
        string ConsumeEscape(Char current)
        {
            if (current.IsHex())
            {
                var escape = new List<Char>();

                for (int i = 0; i < 6; i++)
                {
                    escape.Add(current);
                    current = src.Next;

                    if (!current.IsHex())
                        break;
                }

                current = src.Previous;
                var code = Int32.Parse(new String(escape.ToArray()), NumberStyles.HexNumber);
                return Char.ConvertFromUtf32(code);
            }

            return current.ToString();
        }

        /// <summary>
        /// Checks if the current position is the beginning of a valid escape sequence.
        /// </summary>
        /// <returns>The result of the check.</returns>
        bool IsValidEscape(Char current)
        {
            if (current != Specification.RSOLIDUS)
                return false;

            current = src.Next;
            src.Back();

            if (current == Specification.EOF)
                return false;
            else if (current.IsLineBreak())
                return false;

            return true;
        }

        #endregion
    }

    static class HtmlEncoding
    {
        public const string CHARSET = "charset";

        /// <summary>
        /// Tries to extract the encoding from the given http-equiv content string.
        /// </summary>
        /// <param name="content">The value of the attribute.</param>
        /// <returns>The extracted encoding or an empty string.</returns>
        public static string Extract(string content)
        {
            var position = 0;
            content = content.ToLower();

            for (int i = position; i < content.Length - 7; i++)
            {
                if (content.Substring(i).StartsWith(CHARSET))
                {
                    position = i + 7;
                    break;
                }
            }

            if (position > 0 && position < content.Length)
            {
                for (int i = position; i < content.Length - 1; i++)
                {
                    if (content[i].IsSpaceCharacter())
                        position++;
                    else
                        break;
                }

                if (content[position] != Specification.EQ)
                    return Extract(content.Substring(position));

                position++;

                for (int i = position; i < content.Length; i++)
                {
                    if (content[i].IsSpaceCharacter())
                        position++;
                    else
                        break;
                }

                if (position < content.Length)
                {
                    if (content[position] == Specification.DQ)
                    {
                        content = content.Substring(position + 1);
                        var index = content.IndexOf(Specification.DQ);

                        if (index != -1)
                            return content.Substring(0, index);
                    }
                    else if (content[position] == Specification.SQ)
                    {
                        content = content.Substring(position + 1);
                        var index = content.IndexOf(Specification.SQ);

                        if (index != -1)
                            return content.Substring(0, index);
                    }
                    else
                    {
                        content = content.Substring(position);
                        var index = 0;

                        for (int i = 0; i < content.Length; i++)
                        {
                            if (content[i].IsSpaceCharacter())
                                break;
                            else if (content[i] == ';')
                                break;
                            else
                                index++;
                        }

                        return content.Substring(0, index);
                    }
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// Detects if a valid encoding has been found in the given charset string.
        /// </summary>
        /// <param name="charset">The parsed charset string.</param>
        /// <returns>True if a valid encdoing has been found, otherwise false.</returns>
        public static bool IsSupported(string charset)
        {
            return Resolve(charset) != null;
        }

        /// <summary>
        /// Resolves an Encoding instance given by the charset string.
        /// </summary>
        /// <param name="charset">The charset string.</param>
        /// <returns>An instance of the Encoding class or null.</returns>
        public static Encoding Resolve(string charset)
        {
            charset = charset.ToLower();

            switch (charset)
            {
                case "unicode-1-1-utf-8":
                case "utf-8":
                case "utf8":
                    return Encoding.UTF8;

                case "utf-16be":
                    return Encoding.BigEndianUnicode;

                case "utf-16":
                case "utf-16le":
                    return Encoding.Unicode;

                case "dos-874":
                case "iso-8859-11":
                case "iso8859-11":
                case "iso885911":
                case "tis-620":
                case "windows-874":
                    return Encoding.GetEncoding("windows-874");

                case "cp1250":
                case "windows-1250":
                case "x-cp1250":
                    return Encoding.GetEncoding("windows-1250");

                case "cp1251":
                case "windows-1251":
                case "x-cp1251":
                    return Encoding.GetEncoding("windows-1251");

                case "ansi_x3.4-1968":
                case "ascii":
                case "cp1252":
                case "cp819":
                case "csisolatin1":
                case "ibm819":
                case "iso-8859-1":
                case "iso-ir-100":
                case "iso8859-1":
                case "iso88591":
                case "iso_8859-1":
                case "iso_8859-1:1987":
                case "l1":
                case "latin1":
                case "us-ascii":
                case "windows-1252":
                case "x-cp1252":
                    return Encoding.GetEncoding("windows-1252");

                case "cp1253":
                case "windows-1253":
                case "x-cp1253":
                    return Encoding.GetEncoding("windows-1253");

                case "cp1254":
                case "csisolatin5":
                case "iso-8859-9":
                case "iso-ir-148":
                case "iso8859-9":
                case "iso88599":
                case "iso_8859-9":
                case "iso_8859-9:1989":
                case "l5":
                case "latin5":
                case "windows-1254":
                case "x-cp1254":
                    return Encoding.GetEncoding("windows-1254");

                case "cp1255":
                case "windows-1255":
                case "x-cp1255":
                    return Encoding.GetEncoding("windows-1255");

                case "cp1256":
                case "windows-1256":
                case "x-cp1256":
                    return Encoding.GetEncoding("windows-1256");

                case "cp1257":
                case "windows-1257":
                case "x-cp1257":
                    return Encoding.GetEncoding("windows-1257");

                case "cp1258":
                case "windows-1258":
                case "x-cp1258":
                    return Encoding.GetEncoding("windows-1258");

                case "csmacintosh":
                case "mac":
                case "macintosh":
                case "x-mac-roman":
                    return Encoding.GetEncoding("macintosh");

                case "x-mac-cyrillic":
                case "x-mac-ukrainian":
                    return Encoding.GetEncoding("x-mac-cyrillic");

                case "866":
                case "cp866":
                case "csibm866":
                case "ibm866":
                    return Encoding.GetEncoding("cp866");

                case "csisolatin2":
                case "iso-8859-2":
                case "iso-ir-101":
                case "iso8859-2":
                case "iso88592":
                case "iso_8859-2":
                case "iso_8859-2:1987":
                case "l2":
                case "latin2":
                    return Encoding.GetEncoding("iso-8859-2");

                case "csisolatin3":
                case "iso-8859-3":
                case "iso-ir-109":
                case "iso8859-3":
                case "iso88593":
                case "iso_8859-3":
                case "iso_8859-3:1988":
                case "l3":
                case "latin3":
                    return Encoding.GetEncoding("iso-8859-3");

                case "csisolatin4":
                case "iso-8859-4":
                case "iso-ir-110":
                case "iso8859-4":
                case "iso88594":
                case "iso_8859-4":
                case "iso_8859-4:1988":
                case "l4":
                case "latin4":
                    return Encoding.GetEncoding("iso-8859-4");

                case "csisolatincyrillic":
                case "cyrillic":
                case "iso-8859-5":
                case "iso-ir-144":
                case "iso8859-5":
                case "iso88595":
                case "iso_8859-5":
                case "iso_8859-5:1988":
                    return Encoding.GetEncoding("iso-8859-5");

                case "arabic":
                case "asmo-708":
                case "csiso88596e":
                case "csiso88596i":
                case "csisolatinarabic":
                case "ecma-114":
                case "iso-8859-6":
                case "iso-8859-6-e":
                case "iso-8859-6-i":
                case "iso-ir-127":
                case "iso8859-6":
                case "iso88596":
                case "iso_8859-6":
                case "iso_8859-6:1987":
                    return Encoding.GetEncoding("iso-8859-6");

                case "csisolatingreek":
                case "ecma-118":
                case "elot_928":
                case "greek":
                case "greek8":
                case "iso-8859-7":
                case "iso-ir-126":
                case "iso8859-7":
                case "iso88597":
                case "iso_8859-7":
                case "iso_8859-7:1987":
                case "sun_eu_greek":
                    return Encoding.GetEncoding("iso-8859-7");

                case "csiso88598e":
                case "csisolatinhebrew":
                case "hebrew":
                case "iso-8859-8":
                case "iso-8859-8-e":
                case "iso-ir-138":
                case "iso8859-8":
                case "iso88598":
                case "iso_8859-8":
                case "iso_8859-8:1988":
                case "visual":
                    return Encoding.GetEncoding("iso-8859-8");

                case "csiso88598i":
                case "iso-8859-8-i":
                case "logical":
                    return Encoding.GetEncoding("iso-8859-8-i");

                case "iso-8859-13":
                case "iso8859-13":
                case "iso885913":
                    return Encoding.GetEncoding("iso-8859-13");

                case "csisolatin9":
                case "iso-8859-15":
                case "iso8859-15":
                case "iso885915":
                case "iso_8859-15":
                case "l9":
                    return Encoding.GetEncoding("iso-8859-15");

                case "cskoi8r":
                case "koi":
                case "koi8":
                case "koi8-r":
                case "koi8_r":
                    return Encoding.GetEncoding("koi8-r");

                case "koi8-u":
                    return Encoding.GetEncoding("koi8-u");

                case "chinese":
                case "csgb2312":
                case "csiso58gb231280":
                case "gb2312":
                case "gb_2312":
                case "gb_2312-80":
                case "gbk":
                case "iso-ir-58":
                case "x-gbk":
                    return Encoding.GetEncoding("x-cp20936");

                case "hz-gb-2312":
                    return Encoding.GetEncoding("hz-gb-2312");

                case "gb18030":
                    return Encoding.GetEncoding("GB18030");

                case "big5":
                case "big5-hkscs":
                case "cn-big5":
                case "csbig5":
                case "x-x-big5":
                    return Encoding.GetEncoding("big5");

                case "csiso2022jp":
                case "iso-2022-jp":
                    return Encoding.GetEncoding("iso-2022-jp");

                case "csiso2022kr":
                case "iso-2022-kr":
                    return Encoding.GetEncoding("iso-2022-kr");

                case "iso-2022-cn":
                case "iso-2022-cn-ext":
                    return Encoding.GetEncoding("iso-2022-jp");

                default:
                    return null;
            }
        }

        /// <summary>
        /// Suggests an Encoding for the given local.
        /// </summary>
        /// <param name="local">The local defined by the BCP 47 language tag.</param>
        /// <returns>The suggested encoding.</returns>
        public static Encoding Suggest(string local)
        {
            var firstTwo = local.Substring(0, 2).ToLower();

            switch (firstTwo)
            {
                case "ar":
                case "cy":
                case "fa":
                case "hr":
                case "kk":
                case "mk":
                case "or":
                case "ro":
                case "sr":
                case "vi":
                    return Encoding.UTF8;

                case "be":
                    return Encoding.GetEncoding("iso-8859-5");

                case "bg":
                case "ru":
                case "uk":
                    return Encoding.GetEncoding("windows-1251");

                case "cs":
                case "hu":
                case "pl":
                case "sl":
                    return Encoding.GetEncoding("iso-8859-2");

                case "tr":
                case "ku":
                    return Encoding.GetEncoding("windows-1254");

                case "he":
                    return Encoding.GetEncoding("windows-1255");

                case "lv":
                    return Encoding.GetEncoding("iso-8859-13");

                case "ja"://  Windows-31J ???? Replaced by something better anyway
                    return Encoding.UTF8;

                case "ko":
                    return Encoding.GetEncoding("ks_c_5601-1987");

                case "lt":
                    return Encoding.GetEncoding("windows-1257");

                case "sk":
                    return Encoding.GetEncoding("windows-1250");

                case "th":
                    return Encoding.GetEncoding("windows-874");
            }

            if (local.Equals("zh-CN", StringComparison.OrdinalIgnoreCase))
                return Encoding.GetEncoding("GB18030");
            else if (local.Equals("zh-TW", StringComparison.OrdinalIgnoreCase))
                return Encoding.GetEncoding("big5");

            return Encoding.GetEncoding("windows-1252");
        }
    }
}
