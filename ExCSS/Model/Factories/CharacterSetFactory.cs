using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class CharacterSetFactory : RuleFactory
    {
        public CharacterSetFactory(StyleSheet context) : base( context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var characterSetRule = new CharacterSetRule(Context);

            var segment = reader.Current.GrammarSegment;

            if (segment == GrammarSegment.String || segment == GrammarSegment.Ident)
            {
                var block = reader.Current as SymbolBlock;
                characterSetRule.Encoding = block != null 
                    ? block.Value 
                    : ((StringBlock) reader.Current).Value;
            }

            reader.SkipToNextSemicolon();
           
            Context.Rulesets.Add(characterSetRule);
        }
    }
}
