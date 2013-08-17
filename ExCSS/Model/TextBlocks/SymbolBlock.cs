
namespace ExCSS.Model
{
    internal sealed class SymbolBlock : Block
    {
        SymbolBlock(GrammarSegment type)
        {
            Type = type;
        }
        
        public static SymbolBlock Function(string name)
        {
            return new SymbolBlock(GrammarSegment.Function) { Value = name };
        }

        public static SymbolBlock Ident(string identifier)
        {
            return new SymbolBlock(GrammarSegment.Ident) { Value = identifier };
        }

        public static SymbolBlock At(string name)
        {
            return new SymbolBlock(GrammarSegment.AtRule) { Value = name };
        }

        public static SymbolBlock Hash(string characters)
        {
            return new SymbolBlock(GrammarSegment.Hash) { Value = characters };
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            switch (Type)
            {
                case GrammarSegment.Hash:
                    return "#" + Value;

                case GrammarSegment.AtRule:
                    return "@" + Value;
            }

            return Value;
        }
    }
}
