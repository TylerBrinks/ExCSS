using System;

namespace ExCSS.Model
{
    sealed class SpecialCharacter : CharacterBlock
    {
        static readonly SpecialCharacter colon;
        static readonly SpecialCharacter comma;
        static readonly SpecialCharacter semicolon;
        static readonly SpecialCharacter whitespace;

        static SpecialCharacter()
        {
            colon = new SpecialCharacter(Specification.Colon, GrammarSegment.Colon);
            comma = new SpecialCharacter(Specification.Comma, GrammarSegment.Comma);
            semicolon = new SpecialCharacter(Specification.Simicolon, GrammarSegment.Semicolon);
            whitespace = new SpecialCharacter(Specification.Space, GrammarSegment.Whitespace);
        }

        SpecialCharacter(char c, GrammarSegment type) : base(c)
        {
            Type = type;
        }

        public static SpecialCharacter Colon
        {
            get { return colon; }
        }

        public static SpecialCharacter Comma
        {
            get { return comma; }
        }

        public static SpecialCharacter Semicolon
        {
            get { return semicolon; }
        }

        public static SpecialCharacter Whitespace
        {
            get { return whitespace; }
        }
        
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
