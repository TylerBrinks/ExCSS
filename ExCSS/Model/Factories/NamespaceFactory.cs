using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class NamespaceFactory : RuleFactory
    {
        public NamespaceFactory(StyleSheet context)  : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> reader)
        {
            var namespaceRule = new NamespaceRule(Context);

            if (reader.Current.GrammarSegment == GrammarSegment.Ident)
            {
                namespaceRule.Prefix = reader.Current.ToString();
                reader.SkipToNextNonWhitespace();
            }

            if (reader.Current.GrammarSegment == GrammarSegment.String)
            {
                namespaceRule.Uri = reader.Current.ToString();
            }

            reader.SkipToNextSemicolon();
            Context.AtRules.Add(namespaceRule);
        }
    }
}
