using System.IO;

namespace ExCSS
{
    public sealed class CharsetRule : Rule
    {
        internal CharsetRule(StylesheetParser parser)
            : base(RuleType.Charset, parser)
        {
        }

        public string CharacterSet { get; set; }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(formatter.Rule("@charset", CharacterSet.StylesheetString()));
        }

        protected override void ReplaceWith(IRule rule)
        {
            var newRule = rule as CharsetRule;
            CharacterSet = newRule?.CharacterSet;
            base.ReplaceWith(rule);
        }
    }
}