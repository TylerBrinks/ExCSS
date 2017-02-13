using System.Collections.Generic;

namespace ExCSS
{
    internal static class Symbols
    {
        public static readonly string[] NewLines = { "\r\n", "\r", "\n" };

        public const char EndOfFile = char.MaxValue;
        public const char Tilde = (char) 0x7e;              // ~
        public const char Pipe = (char) 0x7c;               // |
        public const char Null = (char) 0x0;
        public const char Ampersand = (char) 0x26;          // &amp
        public const char Num = (char) 0x23;                // #
        public const char Dollar = (char) 0x24;             // $
        public const char Semicolon = (char) 0x3b;          // ;
        public const char Asterisk = (char) 0x2a;           // *
        public const char Equality = (char) 0x3d;           // =
        public const char Plus = (char) 0x2b;               // +
        public const char Minus = (char) 0x2d;              // -
        public const char Comma = (char) 0x2c;              // ,
        public const char Dot = (char) 0x2e;                // .
        public const char Accent = (char) 0x5e;             // ^
        public const char At = (char) 0x40;                 // @
        public const char LessThan = (char) 0x3c;           // <
        public const char GreaterThan = (char) 0x3e;        // >
        public const char SingleQuote = (char) 0x27;        // '
        public const char DoubleQuote = (char) 0x22;        // "
        public const char CurvedQuote = (char) 0x60;        // `
        public const char QuestionMark = (char) 0x3f;       // ?
        public const char Tab = (char) 0x09;                //tab
        public const char LineFeed = (char) 0x0a;           // line feeed
        public const char CarriageReturn = (char) 0x0d;     //return
        public const char FormFeed = (char) 0x0c;           // form feed
        public const char Space = (char) 0x20;              // space
        public const char Solidus = (char) 0x2f;            // solidus /
        public const char NoBreakSpace = (char) 0xa0;       // no breaking space
        public const char ReverseSolidus = (char) 0x5c;     // reverse solidus \
        public const char Colon = (char) 0x3a;              // :
        public const char ExclamationMark = (char) 0x21;    //!
        public const char Replacement = (char) 0xfffd;      // replacement
        public const char Underscore = (char) 0x5f;         // _
        public const char RoundBracketOpen = (char) 0x28;   // (
        public const char RoundBracketClose = (char) 0x29;  // )
        public const char SquareBracketOpen = (char) 0x5b;  // [
        public const char SquareBracketClose = (char) 0x5d; // ]
        public const char CurlyBracketOpen = (char) 0x7b;   // {
        public const char CurlyBracketClose = (char) 0x7d;  // }
        public const char Percent = (char) 0x25;            // %
        public const int MaximumCodepoint = 0x10FFFF;       //The maximum allowed codepoint (defined in Unicode).
        public static Dictionary<char, char> Punycode = new Dictionary<char, char>
        {
            {'。', '.'},
            {'．', '.'},
            {'Ｇ', 'g'},
            {'ｏ', 'o'},
            {'ｃ', 'c'},
            {'Ｘ', 'x'},
            {'０', '0'},
            {'１', '1'},
            {'２', '2'},
            {'５', '5'}
        };
    }
}