using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class PageRuleFactory : RuleFactory
    {
        public PageRuleFactory(StyleSheetContext context)
            : base(context)
        { }

        public override void Parse(IEnumerator<Block> source)
        {
           
        //private PageRule CreatePageRule(IEnumerator<Block> reader)
       
            var page = new PageRule(Context);

            Context.ActiveRules.Push(page);

            var selector = new SelectorConstructor { /*IgnoreErrors = _ignore */};

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (source.SkipToNextNonWhitespace())
                    {
                        var tokens = source.LimitToCurrentBlock();
                        tokens.GetEnumerator().AppendDeclarations(page.Style.List);
                        break;
                    }
                }

                selector.PickSelector(source);
            }
            while (source.MoveNext());

            page.Selector = selector.Result;
            Context.ActiveRules.Pop();
            //return page;
        }
    }
}
