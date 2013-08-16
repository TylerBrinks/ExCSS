
using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents the @font-face rule.
    /// </summary>
    ////[DOM("FontFaceRule")]
    public sealed class FontFaceRule : Ruleset
    {
        #region Constants

        internal const string RuleName = "font-face";

        #endregion

        #region Members

        StyleDeclaration _cssRules;

        #endregion

        #region ctor

        internal FontFaceRule(StyleSheetContext context)
            : base(context)
        {
            _cssRules = new StyleDeclaration();
            _type = RuleType.FontFace;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Appends the given rule to the list of rules.
        /// </summary>
        /// <param name="rule">The rule to append.</param>
        /// <returns>The current font-face rule.</returns>
        internal FontFaceRule AppendRule(Property rule)
        {
            _cssRules.List.Add(rule);
            return this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the declared CSS rules.
        /// </summary>
        //[DOM("cssRules")]
        public StyleDeclaration CssRules
        {
            get { return _cssRules; }
        }

        /// <summary>
        /// Gets or sets the font-family.
        /// </summary>
        //[DOM("family")]
        public string Family
        {
            get { return _cssRules.GetPropertyValue("font-family"); }
            set { _cssRules.SetProperty("font-family", value); }
        }

        /// <summary>
        /// Gets or sets the source of the font.
        /// </summary>
        //[DOM("src")]
        public string Src
        {
            get { return _cssRules.GetPropertyValue("src"); }
            set { _cssRules.SetProperty("src", value); }
        }

        /// <summary>
        /// Gets or sets the style of the font.
        /// </summary>
        //[DOM("style")]
        public string Style
        {
            get { return _cssRules.GetPropertyValue("font-style"); }
            set { _cssRules.SetProperty("font-style", value); }
        }

        /// <summary>
        /// Gets or sets the weight of the font.
        /// </summary>
        //[DOM("weight")]
        public string Weight
        {
            get { return _cssRules.GetPropertyValue("font-weight"); }
            set { _cssRules.SetProperty("font-weight", value); }
        }

        /// <summary>
        /// Gets or sets the stretch value of the font.
        /// </summary>
        //[DOM("stretch")]
        public string Stretch
        {
            get { return _cssRules.GetPropertyValue("stretch"); }
            set { _cssRules.SetProperty("stretch", value); }
        }

        /// <summary>
        /// Gets or sets the unicode range of the font.
        /// </summary>
        //[DOM("unicodeRange")]
        public string UnicodeRange
        {
            get { return _cssRules.GetPropertyValue("unicode-range"); }
            set { _cssRules.SetProperty("unicode-range", value); }
        }

        /// <summary>
        /// Gets or sets the variant of the font.
        /// </summary>
        //[DOM("variant")]
        public string Variant
        {
            get { return _cssRules.GetPropertyValue("font-variant"); }
            set { _cssRules.SetProperty("font-variant", value); }
        }

        /// <summary>
        /// Gets or sets the feature settings of the font.
        /// </summary>
        //[DOM("featureSettings")]
        public string FeatureSettings
        {
            get { return _cssRules.GetPropertyValue("font-feature-settings"); }
            set { _cssRules.SetProperty("font-feature-settings", value); }
        }

        #endregion

        public override string ToString()
        {
            return "@font-face {" + Environment.NewLine + _cssRules.ToCss() + "}";
        }
    }
}
