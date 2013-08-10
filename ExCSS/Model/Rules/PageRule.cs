
using System;


namespace ExCSS.Model
{
    /// <summary>
    /// Represents the @page rule.
    /// </summary>
    ////[DOM("PageRule")]
    public sealed class PageRule : Ruleset
    {
        #region Constants

        internal const string RuleName = "page";

        #endregion

        #region Members

        StyleDeclaration _style;
        Selector _selector;
        string _selectorText;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new @page rule.
        /// </summary>
        internal PageRule()
        {
            _style = new StyleDeclaration();
            _type = RuleType.Page;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Appends the given rule to the list of rules.
        /// </summary>
        /// <param name="rule">The rule to append.</param>
        /// <returns>The current font-face rule.</returns>
        internal PageRule AppendRule(Property rule)
        {
            _style.List.Add(rule);
            return this;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the selector.
        /// </summary>
        internal Selector Selector
        {
            get { return _selector; }
            set
            {
                _selector = value;
                _selectorText = value.ToString();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parsable textual representation of the page selector for the rule.
        /// </summary>
        //[DOM("selectorText")]
        public string SelectorText
        {
            get { return _selectorText; }
            set
            {
                _selector = Parser.ParseSelector(value);
                _selectorText = value;
            }
        }

        /// <summary>
        /// Gets the  declaration-block of this rule.
        /// </summary>
        //[DOM("style")]
        public StyleDeclaration Style
        {
            get { return _style; }
        }

        #endregion

        public override string ToString()
        {
            return String.Format("@page {0} {{{1}{2}}}", _selectorText, Environment.NewLine, _style);
        }

    }
}
