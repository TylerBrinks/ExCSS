using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ExCSS.Model
{
    internal sealed class StylesheetStreamReader
    {
        private int _insertion;
        private readonly Stack<int> _collengths;
        private TextReader _reader;
        private readonly StringBuilder _buffer;
        private bool _lwcr;
        private Encoding _encoding;

        StylesheetStreamReader()
        {
            _encoding = HtmlEncoding.Suggest(CultureInfo.CurrentUICulture.Name);
            _buffer = new StringBuilder();
            _collengths = new Stack<int>();
            Column = 1;
            Line = 1;
        }

        public StylesheetStreamReader(string source) : this()
        {
            _reader = new StringReader(source);
            ReadCurrent();
        }

        public StylesheetStreamReader(Stream stream) : this()
        {
            _reader = new StreamReader(stream, true);
            ReadCurrent();
        }

        public bool IsBeginning
        {
            get { return _insertion < 2; }
        }

        public Encoding Encoding
        {
            get { return _encoding; }
            set
            {
                _encoding = value;

                if (!(_reader is StreamReader))
                {
                    return;
                }

                var chars = _buffer.Length;
                var stream = ((StreamReader)_reader).BaseStream;
                _insertion = 0;
                stream.Position = 0;

                _buffer.Clear();

                _reader = new StreamReader(stream, value);
                
                Advance(chars);
            }
        }

        public int InsertionPoint
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
                        {
                            BackUnsafe();
                        }
                    }
                    else if (delta < 0)
                    {
                        while (_insertion != value)
                        {
                            AdvanceUnsafe();
                        }
                    }
                }
            }
        }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public bool IsEnded { get; private set; }

        public bool IsEnding
        {
            get { return Current == Specification.EndOfFile; }
        }

        public char Current { get; private set; }

        public char Next
        {
            get
            {
                Advance(); 
                
                return Current;
            }
        }
        
        public char Previous
        {
            get
            {
                Back(); 
                
                return Current;
            }
        }

        public void ResetInsertionPoint()
        {
            InsertionPoint = _buffer.Length;
        }

        public void Advance()
        {
            if (!IsEnding)
            {
                AdvanceUnsafe();
            }
            else if (!IsEnded)
            {
                IsEnded = true;
            }
        }

        public void Advance(int n)
        {
            while (n-- > 0 && !IsEnding)
            {
                AdvanceUnsafe();
            }
        }

        public void Back()
        {
            IsEnded = false;

            if (!IsBeginning)
            {
                BackUnsafe();
            }
        }

        public void Back(int n)
        {
            IsEnded = false;

            while (n-- > 0 && !IsBeginning)
            {
                BackUnsafe();
            }
        }

        public bool ContinuesWith(string s, bool ignoreCase = true)
        {
            for (var index = 0; index < s.Length; index++)
            {
                var chr = Current;

                if (ignoreCase && chr.IsUppercaseAscii())
                {
                    chr = chr.ToLower();
                }

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

        private void ReadCurrent()
        {
            if (_insertion < _buffer.Length)
            {
                Current = _buffer[_insertion];
                _insertion++;
                return;
            }

            var nextPosition = _reader.Read();
            Current = nextPosition == -1 ? Specification.EndOfFile : (char)nextPosition;

            if (Current == Specification.CarriageReturn)
            {
                Current = Specification.LineFeed;
                _lwcr = true;
            }
            else if (_lwcr)
            {
                _lwcr = false;

                if (Current == Specification.LineFeed)
                {
                    ReadCurrent();
                    return;
                }
            }

            _buffer.Append(Current);
            _insertion++;
        }

        private void AdvanceUnsafe()
        {
            if (Current.IsLineBreak())
            {
                _collengths.Push(Column);
                Column = 1;
                Line++;
            }
            else
            {
                Column++;
            }

            ReadCurrent();
        }

        private void BackUnsafe()
        {
            _insertion--;

            if (_insertion == 0)
            {
                Column = 0;
                Current = Specification.Null;
                return;
            }

            Current = _buffer[_insertion - 1];

            if (Current.IsLineBreak())
            {
                Column = _collengths.Count != 0 ? _collengths.Pop() : 1;
                Line--;
            }
            else
            {
                Column--;
            }
        }
    }
}