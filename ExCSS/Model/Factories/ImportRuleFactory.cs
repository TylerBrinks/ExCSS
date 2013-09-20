using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class ImportRuleFactory : RuleFactory
    {
        public ImportRuleFactory(StyleSheet context) : base(context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var import = new ImportRule(Context);

            Context.ActiveRules.Push(import);

            switch (reader.Current.GrammarSegment)
            {
                case GrammarSegment.Semicolon:
                    reader.MoveNext();
                    break;

                case GrammarSegment.String:
                case GrammarSegment.Url:
                    import.Href = ((StringBlock)reader.Current).Value;
                    AppendMediaList(Context, reader, import.Media);

                    break;

                default:
                    reader.SkipToNextSemicolon();
                    break;
            }

            Context.ActiveRules.Pop();
            Context.AtRules.Add(import);
        }

        internal static void AppendMediaList(StyleSheet context, IEnumerator<Block> reader, MediaTypeList media,
            GrammarSegment endToken = GrammarSegment.Semicolon)
        {
            var firstPass = true;
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
