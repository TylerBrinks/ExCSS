using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.Rules;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class CharacterSetFactory : RuleFactory
    {
        public CharacterSetFactory(StyleSheetContext context) : base( context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var characterSetRule = new CharsetRule(Context);

            if (reader.Current.Type == GrammarSegment.String)
            {
                characterSetRule.Encoding = ((StringBlock)reader.Current).Value;
            }

            reader.SkipToNextSemicolon();
           
            Context.AtRules.Add(characterSetRule);
        }
    }
}
