using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.Rules;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class KeyframesFactory : RuleFactory
    {
        public KeyframesFactory(StyleSheetContext context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var keyframes = new KeyframesRule(Context);

            Context.ActiveRules.Push(keyframes);

            if (reader.Current.Type == GrammarSegment.Ident)
            {
                keyframes.Identifier = ((SymbolBlock)reader.Current).Value;
                reader.SkipToNextNonWhitespace();

                if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    reader.SkipToNextNonWhitespace();
                    var tokens = reader.LimitToCurrentBlock().GetEnumerator();

                    while (tokens.SkipToNextNonWhitespace())
                    {
                        keyframes.Declarations.Add(CreateKeyframeRule(tokens));
                    }
                }
            }

            Context.ActiveRules.Pop();
            Context.AtRules.Add(keyframes);
        }

        internal KeyframeRule CreateKeyframeRule(IEnumerator<Block> reader)
        {
            var keyframe = new KeyframeRule(Context);

            Context.ActiveRules.Push(keyframe);

            do
            {
                if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (reader.SkipToNextNonWhitespace())
                    {
                        var tokens = reader.LimitToCurrentBlock();
                        tokens.GetEnumerator().AppendDeclarations(keyframe.Declarations.Properties);
                    }

                    break;
                }

                Context.ReadBuffer.Append(reader.Current);
            }
            while (reader.MoveNext());

            keyframe.Value = Context.ReadBuffer.ToString();
            Context.ReadBuffer.Clear();
            Context.ActiveRules.Pop();

            return keyframe;
        }
    }
}
