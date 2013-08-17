
namespace ExCSS.Model
{
    sealed class SpecialCharacter : CharacterBlock
    {
        private static readonly SpecialCharacter ColonCharacter;
        private static readonly SpecialCharacter CommaCharacter;
        private static readonly SpecialCharacter SemicolonCharacter;
        private static readonly SpecialCharacter WhitespaceCharacter;

        static SpecialCharacter()
        {
            ColonCharacter = new SpecialCharacter(Specification.Colon, GrammarSegment.Colon);
            CommaCharacter = new SpecialCharacter(Specification.Comma, GrammarSegment.Comma);
            SemicolonCharacter = new SpecialCharacter(Specification.Simicolon, GrammarSegment.Semicolon);
            WhitespaceCharacter = new SpecialCharacter(Specification.Space, GrammarSegment.Whitespace);
        }

        SpecialCharacter(char c, GrammarSegment type) : base(c)
        {
            Type = type;
        }

        public static SpecialCharacter Colon
        {
            get { return ColonCharacter; }
        }

        public static SpecialCharacter Comma
        {
            get { return CommaCharacter; }
        }

        public static SpecialCharacter Semicolon
        {
            get { return SemicolonCharacter; }
        }

        public static SpecialCharacter Whitespace
        {
            get { return WhitespaceCharacter; }
        }
        
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
