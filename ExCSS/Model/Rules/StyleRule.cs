using System;
using ExCSS.Model;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class StyleRule : RuleSet, ISupportsSelector, ISupportsDeclarations
    {
        private string _value;
        private SimpleSelector _selector;
        private readonly StyleDeclaration _declarations;

        public StyleRule() : this( new StyleDeclaration())
        {}

        public StyleRule(StyleDeclaration declarations) 
        {
            RuleType = RuleType.Style;
            _declarations = declarations;
        }

        public SimpleSelector Selector
        {
            get { return _selector; }
            set
            {
                _selector = value;
                _value = value.ToString();
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _selector = Parser.ParseSelector(value);
                _value = value;
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
            //var additionalLine = friendlyFormat ? Environment.NewLine : "";
            return _value.NewLineIndent(friendlyFormat, indentation) +
                "{" +
                _declarations.ToString(friendlyFormat, indentation) +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
