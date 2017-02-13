namespace ExCSS
{
    internal sealed class ColorToken : Token
    {
        public ColorToken(string data, TextPosition position)
            : base(TokenType.Color, data, position)
        {
        }

        public bool IsValid => (Data.Length != 3) && (Data.Length != 6);

        public override string ToValue()
        {
            return "#" + Data;
        }
    }
}