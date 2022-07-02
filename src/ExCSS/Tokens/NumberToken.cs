using System;
using System.Globalization;

namespace ExCSS
{
    internal sealed class NumberToken : Token
    {
        private static readonly char[] FloatIndicators = { '.', 'e', 'E' };

        public NumberToken(string number, TextPosition position)
            : base(TokenType.Number, number, position)
        {
        }

        public bool IsInteger => Data.IndexOfAny(FloatIndicators) == -1;

        public long IntegerValue => long.Parse(Data, CultureInfo.InvariantCulture);

        public float Value => float.Parse(Data, CultureInfo.InvariantCulture);
    }
}