using System.Collections.Generic;
using ExCSS.Model.Extensions;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class SupportFactory : RuleFactory
    {
        public SupportFactory(StyleSheetContext context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var supportsRule = new SupportsRule(Context);

            Context.ActiveRules.Push(supportsRule);

            do
            {
                if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (reader.SkipToNextNonWhitespace())
                    {
                        var tokens = reader.LimitToCurrentBlock();

                        Context.BuildRulesets(tokens.GetEnumerator(), supportsRule.Rules);
                    }

                    break;
                }

                Context.ReadBuffer.Append(reader.Current);
            }
            while (reader.MoveNext());

            supportsRule.ConditionText = Context.ReadBuffer.ToString();
            Context.ReadBuffer.Clear();
            Context.ActiveRules.Pop();
 
            Context.AtRules.Add(supportsRule);
        }
    }
}
