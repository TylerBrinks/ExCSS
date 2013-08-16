using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class ImportRuleFactory : RuleFactory
    {
        public ImportRuleFactory(StyleSheetContext context)
            : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> source)
        {
            var import = new ImportRule(Context);

            Context.ActiveRules.Push(import);

            switch (source.Current.Type)
            {
                case GrammarSegment.Semicolon:
                    source.MoveNext();
                    break;

                case GrammarSegment.String:
                case GrammarSegment.Url:
                    import.Href = ((StringBlock)source.Current).Value;
                    MediaRuleFactory.AppendMediaList(Context, source, import.Media);

                    break;

                default:
                    source.SkipToNextSemicolon();
                    break;
            }

            Context.ActiveRules.Pop();
            //return import;
        }
    }
}
