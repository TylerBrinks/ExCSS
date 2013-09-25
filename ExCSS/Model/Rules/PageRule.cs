using System;
using ExCSS.Model.Extensions;
using ExCSS.Model.Factories;
using ExCSS.Model.Values;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class PageRule : RuleSet
    {
        private readonly StyleDeclaration _declarations;
        private SimpleSelector _selector;
        private string _selectorText;

        public PageRule()
            : this(null)
        {

        }

        internal PageRule(StyleSheet context)
            : base(context)
        {
            _declarations = new StyleDeclaration();
            RuleType = RuleType.Page;
        }

        internal PageRule AppendRule(Property rule)
        {
            _declarations.Properties.Add(rule);
            return this;
        }

        public SimpleSelector Selector
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
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var pseudo = string.IsNullOrEmpty(_selectorText)
                             ? ""
                             : ":" + _selectorText;

            var declarations = _declarations.ToString(friendlyFormat, indentation);

            return "@page " +
                pseudo +
                "{" +
                declarations +
                "}".NewLineIndent(friendlyFormat, indentation) +
                Environment.NewLine;
        }
    }
}
