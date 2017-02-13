using System;
using System.IO;
using System.Linq;

namespace ExCSS
{
    internal sealed class KeyframeRule : Rule, IKeyframeRule
    {
        internal KeyframeRule(StylesheetParser parser)
            : base(RuleType.Keyframe, parser)
        {
            AppendChild(new StyleDeclaration(this));
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(formatter.Style(KeyText, Style));
        }

        public string KeyText
        {
            get { return Key.Text; }
            set
            {
                var selector = Parser.ParseKeyframeSelector(value);
                if (selector == null)
                {
                    throw new ParseException("Unable to parse keyframe selector");
                }
                Key = selector;
            }
        }
        public KeyframeSelector Key
        {
            get { return Children.OfType<KeyframeSelector>().FirstOrDefault(); }
            set { ReplaceSingle(Key, value); }
        }
        public StyleDeclaration Style => Children.OfType<StyleDeclaration>().FirstOrDefault();
    }
}