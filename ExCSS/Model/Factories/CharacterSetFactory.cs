using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class CharacterSetFactory : RuleFactory
    {
        public CharacterSetFactory(StyleSheetContext context) : base( context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var characterSetRule = new CharacterSetRule(Context);

            if (reader.Current.GrammarSegment == GrammarSegment.String)
            {
                characterSetRule.Encoding = ((StringBlock)reader.Current).Value;
            }

            reader.SkipToNextSemicolon();
           
            Context.AtRules.Add(characterSetRule);
        }
    }
}
