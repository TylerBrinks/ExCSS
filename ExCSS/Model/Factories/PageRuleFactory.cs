using System.Collections.Generic;
using ExCSS.Model.Extensions;

namespace ExCSS.Model.Factories
{
    internal class PageRuleFactory : RuleFactory
    {
        public PageRuleFactory(StyleSheetContext context) : base(context)
        { }

        public override void Parse(IEnumerator<Block> reader)
        {
            var pageRule = new PageRule(Context);

            Context.ActiveRules.Push(pageRule);

            var selector = new SelectorConstructor();

            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.CurlyBraceOpen)
                {
                    if (reader.SkipToNextNonWhitespace())
                    {
                        var tokens = reader.LimitToCurrentBlock();
                        tokens.GetEnumerator().AppendDeclarations(pageRule.Declarations.Properties);
                        break;
                    }
                }

                selector.AssignSelector(reader);
            }
            while (reader.MoveNext());

            pageRule.Selector = selector.Result;
            Context.ActiveRules.Pop();
            Context.AtRules.Add(pageRule);
        }
    }
}
