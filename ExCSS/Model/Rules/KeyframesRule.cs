using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model.Extensions;
using ExCSS.Model.Factories.AtRuleFactories;

namespace ExCSS.Model.Rules
{
    public class KeyframesRule : RuleSet, IRuleContainer
    {
        private readonly List<RuleSet> _declarations;
        private string _identifier;

        internal KeyframesRule(StyleSheetContext context) : base( context)
        {
            _declarations = new List<RuleSet>();
            RuleType = RuleType.Keyframes;
        }
       
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public List<RuleSet> Declarations
        {
            get { return _declarations; }
        }

        internal KeyframesRule AppendRule(string rule)
        {
            var obj = ParseKeyframeRule(rule);
            _declarations.Add(obj);

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

        internal KeyframesRule DeleteRule(string key)
        {
            for (var i = 0; i < _declarations.Count; i++)
            {
                if (!(_declarations[i] as KeyframeRule).Value.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                _declarations.RemoveAt(i);
                
                break;
            }

            return this;
        }

        internal KeyframeRule FindRule(string key)
        {
            return _declarations.Select(t => t as KeyframeRule).FirstOrDefault(rule => 
                rule.Value.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return String.Format("@keyframes {0} {{{1}{2}}}", _identifier, Environment.NewLine, _declarations);
        }
    }
}
