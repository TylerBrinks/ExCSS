
namespace ExCSS.Model
{
    internal sealed class SpecialCharacter : CharacterBlock
    {
        internal static readonly SpecialCharacter Colon = new SpecialCharacter(Specification.Colon, GrammarSegment.Colon);
        internal static readonly SpecialCharacter Comma = new SpecialCharacter(Specification.Comma, GrammarSegment.Comma);
        internal static readonly SpecialCharacter Semicolon = new SpecialCharacter(Specification.Simicolon, GrammarSegment.Semicolon);
        internal static readonly SpecialCharacter Whitespace = new SpecialCharacter(Specification.Space, GrammarSegment.Whitespace);

        SpecialCharacter(char c, GrammarSegment type) : base(c)
        {
            Type = type;
        }
        
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
