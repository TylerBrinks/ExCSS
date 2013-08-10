using System;


namespace ExCSS.Model
{
    public sealed class StyleRule : Ruleset
    {
        string _selectorText;
        Selector _selector;
        readonly StyleDeclaration _style;

        internal StyleRule()
        {
            _type = RuleType.Style;
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
                _selector = Parser.ParseSelector(value);
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
