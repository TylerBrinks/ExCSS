using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model.Extensions;
using ExCSS.Model.Factories.AtRuleFactories;

namespace ExCSS.Model.Rules
{
    public class KeyframesRule : RuleSet, IRuleContainer
    {
        private readonly List<RuleSet> _rules;
        private string _name;

        internal KeyframesRule(StyleSheetContext context) : base( context)
        {
            _rules = new List<RuleSet>();
            RuleType = RuleType.Keyframes;
        }
       
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<RuleSet> Rules
        {
            get { return _rules; }
        }

        public KeyframesRule AppendRule(string rule)
        {
            var obj = ParseKeyframeRule(rule);
            _rules.Add(obj);

            return this;
        }

        internal KeyframeRule ParseKeyframeRule(string rule)
        {
            var parser = new Parser(rule);

            var it = parser.Lexer.Tokens.GetEnumerator();

            if (it.SkipToNextNonWhitespace())
            {
                //if (it.Current.Type == GrammarSegment.CommentOpen || it.Current.Type == GrammarSegment.CommentClose)
                // throw new DOMException(ErrorCode.SyntaxError);

                return new KeyframesFactory(Context).CreateKeyframeRule(it);
            }

            return null;
        }

        public KeyframesRule DeleteRule(string key)
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                if (!(_rules[i] as KeyframeRule).KeyText.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                _rules.RemoveAt(i);
                
                break;
            }

            return this;
        }

        public KeyframeRule FindRule(string key)
        {
            return _rules.Select(t => t as KeyframeRule).FirstOrDefault(rule => 
                rule.KeyText.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return String.Format("@keyframes {0} {{{1}{2}}}", _name, Environment.NewLine, _rules);
        }
    }
}
