using System.IO;
using System.Linq;


namespace ExCSS
{
    internal sealed class PageRule : Rule, IPageRule
    {
        internal PageRule(StylesheetParser parser)
            : base(RuleType.Page, parser)
        {
            AppendChild(SimpleSelector.All);
            AppendChild(new StyleDeclaration(this));
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            var rules = formatter.Block(Style);
            writer.Write(formatter.Rule("@page", SelectorText, rules));
        }

        public string SelectorText
        {
            get { return Selector.Text; }
            set { Selector = Parser.ParseSelector(value); }
        }

        public ISelector Selector
        {
            get { return Children.OfType<ISelector>().FirstOrDefault(); }
            set { ReplaceSingle(Selector, value); }
        }

        public StyleDeclaration Style => Children.OfType<StyleDeclaration>().FirstOrDefault();
    }
}