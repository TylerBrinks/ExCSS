using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class ImportRuleFactory : RuleFactory
    {
        public ImportRuleFactory(StyleSheetContext context) : base(context)
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
                    MediaRuleFactory.AppendMediaList(Context, reader, import.Media);

                    break;

                default:
                    reader.SkipToNextSemicolon();
                    break;
            }

            Context.ActiveRules.Pop();
            Context.AtRules.Add(import);
        }
    }
}
