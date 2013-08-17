
using System;

namespace ExCSS.Model.Rules
{
    public sealed class KeyframeRule : RuleSet
    {
        private string _keyText;

        internal KeyframeRule(StyleSheetContext context) : base(context)
        {
            Style = new StyleDeclaration();
        }

        public string KeyText
        {
            get { return _keyText; }
            set { _keyText = value; }
        }

        public StyleDeclaration Style { get; private set; }

        public override string ToString()
        {
            return _keyText + " {" + Environment.NewLine + Style.ToCss() + "}";
        }
    }
}
