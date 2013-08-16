namespace ExCSS.Model
{
    static class Specification
    {
        public const char EndOfFile = (char)0x1a;
        public const char Tilde = (char)0x7e;
        public const char Pipe = (char)0x7c;
        public const char Null = (char)0x0;
        public const char Ampersand = (char)0x26;
        public const char Number = (char)0x23;
        public const char DollarSign = (char)0x24;
        public const char Simicolon = (char)0x3b;
        public const char Asterisk = (char)0x2a;
        public const char EqualSign = (char)0x3d;
        public const char PlusSign = (char)0x2b;
        public const char Comma = (char)0x2c;
        public const char Period = (char)0x2e;
        public const char Accent = (char)0x5e;
        public const char At = (char)0x40;
        public const char LessThan = (char)0x3c;
        public const char GreaterThan = (char)0x3e;
        public const char SingleQuote = (char)0x27;
        public const char DoubleQuote = (char)0x22;
        public const char QuestionMark = (char)0x3f;
        public const char Tab = (char)0x09;
        public const char LineFeed = (char)0x0a;
        public const char CarriageReturn = (char)0x0d;
        public const char FormFeed = (char)0x0c;
        public const char Space = (char)0x20;
        public const char Solidus = (char)0x2f;
        public const char ReverseSolidus = (char)0x5c;
        public const char Colon = (char)0x3a;
        public const char Em = (char)0x21;
        public const char MinusSign = (char)0x2d;
        public const char Replacement = (char)0xfffd;
        public const char Underscore = (char)0x5f;
        public const char ParenOpen = (char)0x28;
        public const char ParenClose = (char)0x29;
        public const char Percent = (char)0x25;
        public const int MaxPoint = 0x10FFFF;/// The maximum allowed codepoint (defined in Unicode).

        public static bool IsNonPrintable(this char c)
        {
            return (c >= 0x0 && c <= 0x8) || (c >= 0xe && c <= 0x1f) || (c >= 0x7f && c <= 0x9f);
        }
        
        public static bool IsLetter(this char c)
        {
            return IsUppercaseAscii(c) || IsLowercaseAscii(c);
        }
        
        public static bool IsName(this char c)
        {
            return c >= 0x80 || c.IsLetter() || c == Underscore || c == MinusSign || IsDigit(c);
        }
        
        public static bool IsNameStart(this char c)
        {
            return c >= 0x80 || IsUppercaseAscii(c) || IsLowercaseAscii(c) || c == Underscore;
        }
        
        public static bool IsLineBreak(this char c)
        {
            //line feed, carriage return
            return c == LineFeed || c == CarriageReturn;
        }
        
        public static bool IsSpaceCharacter(this char c)
        {
            //white space, tab, line feed, form feed, carriage return
            return c == Space || c == Tab || c == LineFeed || c == FormFeed || c == CarriageReturn;
        }

        public static bool IsDigit(this char c)
        {
            return c >= 0x30 && c <= 0x39;
        }

        public static bool IsUppercaseAscii(this char c)
        {
            return c >= 0x41 && c <= 0x5a;
        }

        public static bool IsLowercaseAscii(this char c)
        {
            return c >= 0x61 && c <= 0x7a;
        }

        public static bool IsHex(this char c)
        {
            return IsDigit(c) || (c >= 0x41 && c <= 0x46) || (c >= 0x61 && c <= 0x66);
        }
    }
}