
namespace ExCSS
{
    internal sealed class FontFaceRule : DeclarationRule, IFontFaceRule
    {
        internal FontFaceRule(StylesheetParser parser)
            : base(RuleType.FontFace, RuleNames.FontFace, parser)
        {
        }

        protected override Property CreateNewProperty(string name)
        {
            return PropertyFactory.Instance.CreateFont(name);
        }

        string IFontFaceRule.Family
        {
            get { return GetValue(PropertyNames.FontFamily); }
            set { SetValue(PropertyNames.FontFamily, value); }
        }

        string IFontFaceRule.Source
        {
            get { return GetValue(PropertyNames.Src); }
            set { SetValue(PropertyNames.Src, value); }
        }

        string IFontFaceRule.Style
        {
            get { return GetValue(PropertyNames.FontStyle); }
            set { SetValue(PropertyNames.FontStyle, value); }
        }

        string IFontFaceRule.Weight
        {
            get { return GetValue(PropertyNames.FontWeight); }
            set { SetValue(PropertyNames.FontWeight, value); }
        }

        string IFontFaceRule.Stretch
        {
            get { return GetValue(PropertyNames.FontStretch); }
            set { SetValue(PropertyNames.FontStretch, value); }
        }

        string IFontFaceRule.Range
        {
            get { return GetValue(PropertyNames.UnicodeRange); }
            set { SetValue(PropertyNames.UnicodeRange, value); }
        }

        string IFontFaceRule.Variant
        {
            get { return GetValue(PropertyNames.FontVariant); }
            set { SetValue(PropertyNames.FontVariant, value); }
        }

        string IFontFaceRule.Features
        {
            get { return string.Empty; }
            set { }
        }
    }
}