using System;

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
            return _value + " {" + Environment.NewLine + Declarations + "}";
        }
    }
}
