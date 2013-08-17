using System;
using ExCSS.Model.Factories;

namespace ExCSS.Model
{
    public sealed class PageRule : RuleSet
    {
        private readonly StyleDeclaration _style;
        private Selector _selector;
        private string _selectorText;

        internal PageRule(StyleSheetContext context) : base(context)
        {
            _style = new StyleDeclaration();
            RuleType = RuleType.Page;
        }

        internal PageRule AppendRule(Property rule)
        {
            _style.List.Add(rule);
            return this;
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
            return String.Format("@page {0} {{{1}{2}}}", _selectorText, Environment.NewLine, _style);
        }
    }
}
