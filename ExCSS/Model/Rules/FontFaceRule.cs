using System;

namespace ExCSS.Model
{
    public sealed class FontFaceRule : RuleSet
    {
        private readonly StyleDeclaration _rules;

        internal FontFaceRule(StyleSheetContext context) : base(context)
        {
            _rules = new StyleDeclaration();
            RuleType = RuleType.FontFace;
        }

        internal FontFaceRule AppendRule(Property rule)
        {
            _rules.List.Add(rule);
            return this;
        }

        public StyleDeclaration Rules
        {
            get { return _rules; }
        }

        public string Family
        {
            get { return _rules.GetPropertyValue("font-family"); }
            set { _rules.SetProperty("font-family", value); }
        }

        public string Src
        {
            get { return _rules.GetPropertyValue("src"); }
            set { _rules.SetProperty("src", value); }
        }

        public string Style
        {
            get { return _rules.GetPropertyValue("font-style"); }
            set { _rules.SetProperty("font-style", value); }
        }

        public string Weight
        {
            get { return _rules.GetPropertyValue("font-weight"); }
            set { _rules.SetProperty("font-weight", value); }
        }

        public string Stretch
        {
            get { return _rules.GetPropertyValue("stretch"); }
            set { _rules.SetProperty("stretch", value); }
        }

        public string UnicodeRange
        {
            get { return _rules.GetPropertyValue("unicode-range"); }
            set { _rules.SetProperty("unicode-range", value); }
        }

        public string Variant
        {
            get { return _rules.GetPropertyValue("font-variant"); }
            set { _rules.SetProperty("font-variant", value); }
        }

        public string FeatureSettings
        {
            get { return _rules.GetPropertyValue("font-feature-settings"); }
            set { _rules.SetProperty("font-feature-settings", value); }
        }

        public override string ToString()
        {
            return "@font-face {" + Environment.NewLine + _rules.ToCss() + "}";
        }
    }
}
