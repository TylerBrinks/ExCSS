using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model.Extensions;
using ExCSS.Model.Factories;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class KeyframesRule : RuleSet, IRuleContainer
    {
        private readonly List<RuleSet> _declarations;
        private string _identifier;

        public KeyframesRule() : this(null)
        {
            
        }

        internal KeyframesRule(StyleSheet context) : base(context)
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
            var keyframeRule = ParseKeyframeRule(rule);
            _declarations.Add(keyframeRule);

            return this;
        }

        internal KeyframeRule ParseKeyframeRule(string rule)
        {
            var lexer = new Lexer(new StylesheetReader(rule));

            var enumerator = lexer.Tokens.GetEnumerator();

            return enumerator.SkipToNextNonWhitespace() 
                ? new KeyframesFactory(Context).CreateKeyframeRule(enumerator) 
                : null;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var prefix = friendlyFormat ? Environment.NewLine : "";
            var declarationList = _declarations.Select(d => prefix + d.ToString(friendlyFormat, indentation + 1));
            var declarations = string.Join(" ", declarationList);
            
            return "@keyframes " +
                _identifier +
                "{" +
                declarations +
                "}".NewLineIndent(friendlyFormat, indentation) +
                Environment.NewLine;
        }
    }
}
