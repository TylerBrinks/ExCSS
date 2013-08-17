using System;
using ExCSS.Model.Factories;

namespace ExCSS.Model
{
    public class StyleRule : RuleSet
    {
        private string _selectorText;
        private Selector _selector;
        private readonly StyleDeclaration _style;

        internal StyleRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Style;
            _style = new StyleDeclaration();
        }

        internal Selector Selector
        {
            get { return _selector; }
            set
            {
                _selector = value;
                _selectorText = value.ToString();
            }
        }

        public string SelectorText
        {
            get { return _selectorText; }
            set
            {
                _selector = RuleFactory.ParseSelector(value);
                _selectorText = value;
            }
        }

        public StyleDeclaration Style
        {
            get { return _style; }
        }

        public override string ToString()
        {
            return _selectorText + " {" + Environment.NewLine + _style + "}";
        }
    }
}
