using System;
using ExCSS.Model.Values;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class FontFaceRule : RuleSet
    {
        private readonly StyleDeclaration _declarations;

        public FontFaceRule() : this(null)
        {}

        internal FontFaceRule(StyleSheetContext context) : base(context)
        {
            _declarations = new StyleDeclaration();
            RuleType = RuleType.FontFace;
        }

        internal FontFaceRule AppendRule(Property rule)
        {
            _declarations.Properties.Add(rule);
            return this;
        }

        public StyleDeclaration Declarations
        {
            get { return _declarations; }
        }

        public string FontFamily
        {
            get { return _declarations.GetPropertyValue("font-family"); }
            set { _declarations.SetProperty("font-family", value); }
        }

        public string Src
        {
            get { return _declarations.GetPropertyValue("src"); }
            set { _declarations.SetProperty("src", value); }
        }

        public string FontStyle
        {
            get { return _declarations.GetPropertyValue("font-style"); }
            set { _declarations.SetProperty("font-style", value); }
        }

        public string FontWeight
        {
            get { return _declarations.GetPropertyValue("font-weight"); }
            set { _declarations.SetProperty("font-weight", value); }
        }

        public string Stretch
        {
            get { return _declarations.GetPropertyValue("stretch"); }
            set { _declarations.SetProperty("stretch", value); }
        }

        public string UnicodeRange
        {
            get { return _declarations.GetPropertyValue("unicode-range"); }
            set { _declarations.SetProperty("unicode-range", value); }
        }

        public string FontVariant
        {
            get { return _declarations.GetPropertyValue("font-variant"); }
            set { _declarations.SetProperty("font-variant", value); }
        }

        public string FeatureSettings
        {
            get { return _declarations.GetPropertyValue("font-feature-settings"); }
            set { _declarations.SetProperty("font-feature-settings", value); }
        }

        public override string ToString()
        {
            return "@font-face {" + Environment.NewLine + _declarations + "}";
        }
    }
}
