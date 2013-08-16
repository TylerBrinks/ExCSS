using System.Collections.Generic;
using ExCSS.Model.Rules;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class MediaRuleFactory : RuleFactory
    {
        public MediaRuleFactory(StyleSheetContext context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> source)
        {
            var media = new MediaRule(Context);
            Context.ActiveRules.Push(media);

            AppendMediaList(Context, source, media.Media, GrammarSegment.CurlyBraceOpen);

            if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
            {
                if (source.SkipToNextNonWhitespace())
                {
                    source.LimitToCurrentBlock();
                    Context.AppendRules(media.Rules);
                }
            }

            Context.ActiveRules.Pop();
            
            Context.AtRules.Add(media);
        }

        internal static void AppendMediaList(StyleSheetContext context, IEnumerator<Block> source, MediaQueries media, GrammarSegment endToken = GrammarSegment.Semicolon)
        {
            var firstPass = true;
            do
            {
                if (source.Current.Type == GrammarSegment.Whitespace)
                {
                    continue;
                }

                if (source.Current.Type == endToken)
                {
                    break;
                }

                do
                {
                    if (source.Current.Type == GrammarSegment.Comma || source.Current.Type == endToken)
                    {
                        break;
                    }

                    if (source.Current.Type == GrammarSegment.Whitespace)
                    {
                        context.ReadBuffer.Append(' ');
                    }
                    else
                    {
                        if (!firstPass)
                        {
                            context.ReadBuffer.Append(source.Current);
                        }
                        firstPass = false;
                    }
                }
                while (source.MoveNext());

                if (context.ReadBuffer.Length > 0)
                {
                    media.AppendMedium(context.ReadBuffer.ToString());
                }

                context.ReadBuffer.Clear();

                if (source.Current.Type == endToken)
                {
                    break;
                }
            }
            while (source.MoveNext());
        }
    }
}
