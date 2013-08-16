using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class NamespaceFactory : RuleFactory
    {
        public NamespaceFactory(StyleSheetContext context)
            : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> source)
        {
            var namespaceRule = new NamespaceRule(Context);

            if (source.Current.Type == GrammarSegment.Ident)
            {
                namespaceRule.Prefix = source.Current.ToString();
                source.SkipToNextNonWhitespace();
            }

            if (source.Current.Type == GrammarSegment.String)
            {
                namespaceRule.NamespaceURI = source.Current.ToString();
            }

            source.SkipToNextSemicolon();
            Context.AtRules.Add(namespaceRule);
        }
    }
}
