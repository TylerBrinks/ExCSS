using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.Rules;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class MediaRuleFactory : RuleFactory
    {
        public MediaRuleFactory(StyleSheetContext context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var media = new MediaRule(Context);
            Context.ActiveRules.Push(media);

            AppendMediaList(Context, reader, media.Media, GrammarSegment.CurlyBraceOpen);

            if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
            {
                if (reader.SkipToNextNonWhitespace())
                {
                    reader.LimitToCurrentBlock();
                    Context.BuildRulesets(media.Rules);
                }
            }

            Context.ActiveRules.Pop();
            
            Context.AtRules.Add(media);
        }

        internal static void AppendMediaList(StyleSheetContext context, IEnumerator<Block> reader, MediaQueries media, 
            GrammarSegment endToken = GrammarSegment.Semicolon)
        {
            var firstPass = true;
            do
            {
                if (reader.Current.Type == GrammarSegment.Whitespace)
                {
                    continue;
                }

                if (reader.Current.Type == endToken)
                {
                    break;
                }

                do
                {
                    if (reader.Current.Type == GrammarSegment.Comma || reader.Current.Type == endToken)
                    {
                        break;
                    }

                    if (reader.Current.Type == GrammarSegment.Whitespace)
                    {
                        context.ReadBuffer.Append(' ');
                    }
                    else
                    {
                        if (!firstPass)
                        {
                            context.ReadBuffer.Append(reader.Current);
                        }
                        firstPass = false;
                    }
                }
                while (reader.MoveNext());

                if (context.ReadBuffer.Length > 0)
                {
                    media.AppendMedium(context.ReadBuffer.ToString());
                }

                context.ReadBuffer.Clear();

                if (reader.Current.Type == endToken)
                {
                    break;
                }
            }
            while (reader.MoveNext());
        }
    }
}
