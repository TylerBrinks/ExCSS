using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class StyleRuleFactory : RuleFactory
    {
        internal StyleRuleFactory(StyleSheet context) : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> reader)
        {
            var style = new StyleRule(Context);
            var selector = new SelectorConstructor();

            Context.ActiveRules.Push(style);

            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.CurlyBraceOpen)
                {
                    if (reader.SkipToNextNonWhitespace())
                    {
                        var tokens = reader.LimitToCurrentBlock();
                        tokens.GetEnumerator().AppendDeclarations(style.Declarations.Properties);
                    }

                    break;
                }

                selector.AssignSelector(reader);
            }
            while (reader.MoveNext());

            style.Selector = selector.Result;
            Context.ActiveRules.Pop();

            Context.AppendStyleToActiveRule(style);
        }
    }
}
