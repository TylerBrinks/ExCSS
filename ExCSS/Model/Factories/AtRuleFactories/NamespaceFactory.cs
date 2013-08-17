using System.Collections.Generic;
using ExCSS.Model.Extensions;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class NamespaceFactory : RuleFactory
    {
        public NamespaceFactory(StyleSheetContext context)  : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> reader)
        {
            var namespaceRule = new NamespaceRule(Context);

            if (reader.Current.Type == GrammarSegment.Ident)
            {
                namespaceRule.Prefix = reader.Current.ToString();
                reader.SkipToNextNonWhitespace();
            }

            if (reader.Current.Type == GrammarSegment.String)
            {
                namespaceRule.Uri = reader.Current.ToString();
            }

            reader.SkipToNextSemicolon();
            Context.AtRules.Add(namespaceRule);
        }
    }
}
