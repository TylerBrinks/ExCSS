using System.Collections.Generic;
using ExCSS.Model.Rules;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class AtRuleFactory : RuleFactory
    {
        public AtRuleFactory(StyleSheetContext context)
            : base( context)
        {
        }

        public override void Parse(IEnumerator<Block> source)
        {
            var name = ((SymbolBlock)source.Current).Value;
            source.SkipToNextNonWhitespace();
            IRuleFactory factory;

            switch (name)
            {
                case MediaRule.RuleName:
                    factory = new MediaRuleFactory(Context);
                    break;

                case PageRule.RuleName:
                    factory = new PageRuleFactory( Context);
                    break;

                case ImportRule.RuleName:
                    factory = new ImportRuleFactory( Context);
                    break;

                case FontFaceRule.RuleName:
                    factory = new FontFaceFactory( Context);
                    break;

                case CharsetRule.RuleName:
                    factory = new CharacterSetFactory( Context);
                    break;

                case NamespaceRule.RuleName:
                    factory = new NamespaceFactory( Context);
                    break;

                case SupportsRule.RuleName:
                    factory = new SupportFactory( Context);
                    break;

                case KeyframesRule.RuleName:
                    factory = new KeyframesFactory( Context);
                    break;

                default:
                    factory = new UnknownAtRuleFactory( Context);
                    break;
            }

            factory.Parse(source);
        }
    }
}