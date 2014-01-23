using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class FontFaceFactory:RuleFactory
    {
        public FontFaceFactory(StyleSheet context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var fontface = new FontFaceRule(Context);

            Context.ActiveRules.Push(fontface);

            if (reader.Current.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                if (reader.SkipToNextNonWhitespace())
                {
                    var tokens = reader.LimitToCurrentBlock();
                    tokens.GetEnumerator().AppendDeclarations(fontface.Declarations.Properties, Context.Lexer.ErrorHandler);
                }
            }

            Context.ActiveRules.Pop();

            Context.Rulesets.Add(fontface);
        }
    }
}
