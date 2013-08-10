
using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents the GroupingRule interface.
    /// </summary>
    public abstract class GroupingRule : Ruleset
    {
        #region Members

        RuleList _rules;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS grouping rule.
        /// </summary>
        internal GroupingRule()
        {
            _rules = new RuleList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of all CSS rules contained within the media block.
        /// </summary>
        //[DOM("cssRules")]
        public RuleList Rules
        {
            get { return _rules; }
        }

        #endregion

    }
}
