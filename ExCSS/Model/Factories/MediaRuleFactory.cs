using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class MediaRuleFactory : RuleFactory
    {
        public MediaRuleFactory(StyleSheet context)
            : base(context)
        { }

        public override void Parse(IEnumerator<Block> reader)
        {
            var media = new MediaRule(Context);
            Context.ActiveRules.Push(media);

            AppendMediaList(Context, reader, media.Media, GrammarSegment.CurlyBraceOpen);

            if (reader.Current.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                if (reader.SkipToNextNonWhitespace())
                {
                    var tokens = reader.LimitToCurrentBlock();
                    
                    //Context.BuildRulesets(media.Declarations);
                    Context.BuildRulesets(tokens.GetEnumerator(), media.Declarations);
                }
            }

            Context.ActiveRules.Pop();
            Context.Rulesets.Add(media);
        }

        internal static void AppendMediaList(StyleSheet context, IEnumerator<Block> reader, MediaTypeList media,
            GrammarSegment endToken = GrammarSegment.Semicolon)
        {
            //var firstPass = true;
            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.Whitespace)
                {
                    continue;
                }

                if (reader.Current.GrammarSegment == endToken)
                {
                    break;
                }

                do
                {
                    if (reader.Current.GrammarSegment == GrammarSegment.Comma || reader.Current.GrammarSegment == endToken)
                    {
                        break;
                    }

                    if (reader.Current.GrammarSegment == GrammarSegment.Whitespace)
                    {
                        // Don't prepend empty characters.
                        if (context.ReadBuffer.Length > 0)
                        {
                            context.ReadBuffer.Append(' ');
                        }
                    }
                    else
                    {
                        //TODO:  first pass?
                        //if (!firstPass)
                        //{
                            context.ReadBuffer.Append(reader.Current);
                        //}
                        //firstPass = false;
                    }
                }
                while (reader.MoveNext());

                if (context.ReadBuffer.Length > 0)
                {
                    media.AppendMedium(context.ReadBuffer.ToString());
                }

                context.ReadBuffer.Clear();

                if (reader.Current.GrammarSegment != endToken)
                {
                    continue;
                }

                // Exiting.  Trim media names
                for (var i = 0; i < media.Count; i++)
                {
                    media[i] = media[i].Trim();
                }
                break;
            }
            while (reader.MoveNext());
        }
    }
}
