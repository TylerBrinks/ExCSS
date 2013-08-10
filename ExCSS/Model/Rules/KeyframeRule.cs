
using System;

namespace ExCSS.Model
{
    public sealed class KeyframeRule : Ruleset
    {
        #region Members

        string _keyText;
        StyleDeclaration _style;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new @keyframe rule.
        /// </summary>
        internal KeyframeRule()
        {
            _style = new StyleDeclaration();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the key of the keyframe, like '10%', '75%'. The from keyword maps to '0%' and the to keyword maps to '100%'.
        /// </summary>
        //[DOM("keyText")]
        public string KeyText
        {
            get { return _keyText; }
            set { _keyText = value; }
        }

        /// <summary>
        /// Gets a CSSStyleDeclaration of the CSS style associated with the key from.
        /// </summary>
        //[DOM("style")]
        public StyleDeclaration Style
        {
            get { return _style; }
        }

        #endregion

        public override string ToString()
        {
            return _keyText + " {" + Environment.NewLine + _style.ToCss() + "}";
        }
    }
}
