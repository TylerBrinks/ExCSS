
using System;

namespace ExCSS
{
    public sealed class KeyframeRule : RuleSet
    {
        private string _value;

        internal KeyframeRule(StyleSheetContext context) : base(context)
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
