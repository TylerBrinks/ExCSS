using System;

namespace ExCSS.Model
{
    public sealed class SupportsRule : ConditionRule
    {
        #region Constants

        internal const string RuleName = "supports";

        #endregion

        #region Members

        string _conditionText;

        #endregion

        #region ctor

        internal SupportsRule(StyleSheetContext context)
            : base(context)
        {
            _type = RuleType.Supports;
            _conditionText = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text of the condition of the support rule.
        /// </summary>
        //[DOM("conditionText")]
        public override string ConditionText
        {
            get { return _conditionText; }
            set { _conditionText = value; }
        }

        #endregion

        public override string ToString()
        {
            return String.Format("@supports {0} {{{1}{2}}}", _conditionText, Environment.NewLine, Rules);
        }
    }
}
