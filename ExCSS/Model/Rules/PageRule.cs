using System;
using ExCSS.Model.Factories;

namespace ExCSS.Model
{
    public sealed class PageRule : RuleSet
    {
        private readonly StyleDeclaration _declarations;
        private SimpleSelector _selector;
        private string _selectorText;

        internal PageRule(StyleSheetContext context) : base(context)
        {
            _declarations = new StyleDeclaration();
            RuleType = RuleType.Page;
        }

        internal PageRule AppendRule(Property rule)
        {
            _declarations.Properties.Add(rule);
            return this;
        }

        internal SimpleSelector Selector
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

        public StyleDeclaration Declarations
        {
            get { return _declarations; }
        }

        public override string ToString()
        {
            return String.Format("@page {0} {{{1}{2}}}", _selectorText, Environment.NewLine, _declarations);
        }
    }
}
