using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class SupportFactory : RuleFactory
    {
        public SupportFactory(StyleSheet context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var supportsRule = new SupportsRule(Context);

            Context.ActiveRules.Push(supportsRule);

            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.CurlyBraceOpen)
                {
                    if (reader.SkipToNextNonWhitespace())
                    {
                        var tokens = reader.LimitToCurrentBlock();

                        Context.BuildRulesets(tokens.GetEnumerator(), supportsRule.Declarations);
                    }

                    break;
                }

                Context.ReadBuffer.Append(reader.Current);
            }
            while (reader.MoveNext());

            supportsRule.Condition = Context.ReadBuffer.ToString();
            Context.ReadBuffer.Clear();
            Context.ActiveRules.Pop();
 
            Context.AtRules.Add(supportsRule);
        }
    }
}
