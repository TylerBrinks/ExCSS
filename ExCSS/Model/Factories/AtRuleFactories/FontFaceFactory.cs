using System.Collections.Generic;
using ExCSS.Model.Extensions;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class FontFaceFactory:RuleFactory
    {
        public FontFaceFactory(StyleSheetContext context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var fontface = new FontFaceRule(Context);

            Context.ActiveRules.Push(fontface);

            if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
            {
                if (reader.SkipToNextNonWhitespace())
                {
                    var tokens = reader.LimitToCurrentBlock();
                    tokens.GetEnumerator().AppendDeclarations(fontface.Rules.List);
                }
            }

            Context.ActiveRules.Pop();

            Context.AtRules.Add(fontface);
        }
    }
}
