using System;
using System.Text;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents the CSS @charset rule.
    /// </summary>
    ////[DOM("CharsetRule")]
    public sealed class CharsetRule : Ruleset
    {
        #region Constants

        internal const string RuleName = "charset";

        #endregion

        #region ctor

        internal CharsetRule()
        {
            _type = RuleType.Charset;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the encoding information set by this rule.
        /// </summary>
        //[DOM("encoding")]
        public string Encoding
        {
            get;
            internal set;
        }

        #endregion

       public override string ToString()
        {
            return String.Format("@charset '{0}';", Encoding);
        }

    }
}
