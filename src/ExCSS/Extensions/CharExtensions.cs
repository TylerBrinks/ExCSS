namespace ExCSS
{
#if !NET40
    using System.Runtime.CompilerServices;
#endif

    internal static class CharExtensions
    {
        public static int FromHex(this char c)
        {
            return c.IsDigit() ? c - 0x30 : c - (c.IsLowercaseAscii() ? 0x57 : 0x37);
        }

        public static string ToHex(this char character)
        {
            return ((int) character).ToString("x");
        }

        public static bool IsInRange(this char c, int lower, int upper)
        {
            return (c >= lower) && (c <= upper);
        }

        public static bool IsNormalQueryCharacter(this char c)
        {
            return c.IsInRange(0x21, 0x7e) && (c != Symbols.DoubleQuote) &&
                   (c != Symbols.CurvedQuote) && (c != Symbols.Num) &&
                   (c != Symbols.LessThan) && (c != Symbols.GreaterThan);
        }

        public static bool IsNormalPathCharacter(this char c)
        {
            return c.IsInRange(0x20, 0x7e) && (c != Symbols.DoubleQuote) &&
                   (c != Symbols.CurvedQuote) && (c != Symbols.Num) &&
                   (c != Symbols.LessThan) && (c != Symbols.GreaterThan) &&
                   (c != Symbols.Space) && (c != Symbols.QuestionMark);
        }

        public static bool IsUppercaseAscii(this char c)
        {
            return (c >= 0x41) && (c <= 0x5a);
        }

        public static bool IsLowercaseAscii(this char c)
        {
            return (c >= 0x61) && (c <= 0x7a);
        }

        public static bool IsAlphanumericAscii(this char c)
        {
            return c.IsDigit() || c.IsUppercaseAscii() || c.IsLowercaseAscii();
        }

        public static bool IsHex(this char c)
        {
            return c.IsDigit() || ((c >= 0x41) && (c <= 0x46)) ||
                ((c >= 0x61) && (c <= 0x66));
        }

        public static bool IsNonAscii(this char c)
        {
            return (c != Symbols.EndOfFile) && (c >= 0x80);
        }

        public static bool IsNonPrintable(this char c)
        {
            return ((c >= 0x0) && (c <= 0x8)) || ((c >= 0xe) && (c <= 0x1f)) ||
                ((c >= 0x7f) && (c <= 0x9f));
        }

        public static bool IsLetter(this char c)
        {
            return IsUppercaseAscii(c) || IsLowercaseAscii(c);
        }

        public static bool IsName(this char c)
        {
            return c.IsNonAscii() || c.IsLetter() || (c == Symbols.Underscore) || (c == Symbols.Minus) || c.IsDigit();
        }

        public static bool IsNameStart(this char c)
        {
            return c.IsNonAscii() || c.IsUppercaseAscii() || c.IsLowercaseAscii() || (c == Symbols.Underscore);
        }

        public static bool IsLineBreak(this char c)
        {
            return (c == Symbols.LineFeed) || (c == Symbols.CarriageReturn);
        }

        public static bool IsSpaceCharacter(this char c)
        {
            return (c == Symbols.Space) || (c == Symbols.Tab) || (c == Symbols.LineFeed) ||
                   (c == Symbols.CarriageReturn) || (c == Symbols.FormFeed);
        }
        
        public static bool IsDigit(this char c)
        {
            return (c >= 0x30) && (c <= 0x39);
        }

        public static bool IsInvalid(this int c)
        {
            return (c == 0) || (c > Symbols.MaximumCodepoint) || ((c > 0xD800) && (c < 0xDFFF));
        }

        public static bool IsOneOf(this char c, char a, char b)
        {
            return (a == c) || (b == c);
        }

        public static bool IsOneOf(this char c, char o1, char o2, char o3)
        {
            return (c == o1) || (c == o2) || (c == o3);
        }

        public static bool IsOneOf(this char c, char o1, char o2, char o3, char o4)
        {
            return (c == o1) || (c == o2) || (c == o3) || (c == o4);
        }
    }
}