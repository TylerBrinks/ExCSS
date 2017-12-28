using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExCSS
{
    public sealed class Stylesheet : StylesheetNode
    {
        private readonly StylesheetParser _parser;

        internal Stylesheet(StylesheetParser parser)
        {
            _parser = parser;
            Rules = new RuleList(this);
        }

        internal RuleList Rules { get; }

        public IEnumerable<IRule> CharacterSetRules => Rules.Where(r => r is CharsetRule);
        public IEnumerable<IRule> FontfaceSetRules => Rules.Where(r => r is FontFaceRule);
        public IEnumerable<IRule> MediaRules => Rules.Where(r => r is MediaRule);
        public IEnumerable<IRule> ImportRules => Rules.Where(r => r is ImportRule);
        public IEnumerable<IRule> NamespaceRules => Rules.Where(r => r is NamespaceRule);
        public IEnumerable<IRule> PageRules => Rules.Where(r => r is PageRule);
        public IEnumerable<IStyleRule> StyleRules => Rules.OfType<IStyleRule>();

        public IRule Add(RuleType ruleType)
        {
            var rule = _parser.CreateRule(ruleType);
            Rules.Add(rule);
            return rule;
        }

        public void RemoveAt(int index)
        {
            Rules.RemoveAt(index);
        }

        public int Insert(string ruleText, int index)
        {
            var rule = _parser.ParseRule(ruleText);
            rule.Owner = this;
            Rules.Insert(index, rule);

            return index;
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(formatter.Sheet(Rules));
        }
    }
}