using System.Collections.Generic;
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
            var declarations = string.Join(" ", _declarations);
            
            return string.Format("@keyframes {0} {{{1}}}", _identifier, declarations);
        }
    }
}
