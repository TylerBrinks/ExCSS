using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents the abstract base class for
    /// CSS media and CSS supports rules.
    /// </summary>
    public abstract class ConditionRule : GroupingRule
    {
        #region ctor

        /// <summary>
        /// Constructs a new CSS condition rule.
        /// </summary>
        internal ConditionRule ()
	    { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text of the condition of the rule.
        /// </summary>
        //[DOM("conditionText")]
        public virtual string ConditionText
        {
            get { return String.Empty; }
            set { }
        }

        #endregion
    }
}
