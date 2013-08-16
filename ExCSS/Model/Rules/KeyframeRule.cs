
using System;

namespace ExCSS.Model
{
    public sealed class KeyframeRule : Ruleset
    {
        private string _keyText;
        private StyleDeclaration _style;

        internal KeyframeRule(StyleSheetContext context)
            : base(context)
        {
            _style = new StyleDeclaration();
        }

        public string KeyText
        {
            get { return _keyText; }
            set { _keyText = value; }
        }

        public StyleDeclaration Style
        {
            get { return _style; }
        }

        public override string ToString()
        {
            return _keyText + " {" + Environment.NewLine + _style.ToCss() + "}";
        }
    }
}
