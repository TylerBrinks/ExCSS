using System;
using ExCSS.Model.Extensions;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class KeyframeRule : RuleSet
    {
        private string _value;

        public KeyframeRule() : this(null)
        {
            
        }

        internal KeyframeRule(StyleSheet context) : base(context)
        {
            Declarations = new StyleDeclaration();
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public StyleDeclaration Declarations { get; private set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.Empty.Indent(friendlyFormat, indentation) +
                _value + 
                "{" + 
                Declarations.ToString(friendlyFormat, indentation) +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
