
using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents an @keyframes rule.
    /// </summary>
    ////[DOM("KeyframesRule")]
    public sealed class KeyframesRule : Ruleset
    {
        #region Constants

        internal const string RuleName = "keyframes";

        #endregion

        #region Members

        RuleList _rules;
        string _name;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new @keyframes rule.
        /// </summary>
        internal KeyframesRule()
        {
            _rules = new RuleList();
            _type = RuleType.Keyframes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the animation, used by the animation-name property.
        /// </summary>
        //[DOM("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets a RuleList of the CSS rules in the media rule.
        /// </summary>
        //[DOM("cssRules")]
        public RuleList Rules
        {
            get { return _rules; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a new keyframe rule into the current KeyframesRule.
        /// </summary>
        /// <param name="rule">A string containing a keyframe in the same format as an entry of a @keyframes at-rule.</param>
        /// <returns>The current @keyframes rule.</returns>
        //[DOM("appendRule")]
        public KeyframesRule AppendRule(string rule)
        {
            var obj = Parser.ParseKeyframeRule(rule);
     
            obj.ParentRule = this;
            //_rules.InsertAt(_rules.Length, obj);
            _rules.Add(obj);
            return this;
        }

        /// <summary>
        /// Deletes a keyframe rule from the current KeyframesRule. 
        /// </summary>
        /// <param name="key">The index of the keyframe to be deleted, expressed as a string resolving as a number between 0 and 1.</param>
        /// <returns>The current @keyframes rule.</returns>
        //[DOM("deleteRule")]
        public KeyframesRule DeleteRule(string key)
        {
            //for (int i = 0; i < _rules.Length; i++)
            for (var i = 0; i < _rules.Count; i++)
            {
                if ((_rules[i] as KeyframeRule).KeyText.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    _rules.RemoveAt(i);
                    break;
                }
            }

            return this;
        }

        public KeyframeRule FindRule(string key)
        {
            //for (int i = 0; i < _rules.Length; i++)
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i] as KeyframeRule;

                if (rule.KeyText.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return rule;
                }
            }

            return null;
        }

        #endregion

       public override string ToString()
        {
            return String.Format("@keyframes {0} {{{1}{2}}}", _name, Environment.NewLine, _rules);
        }

    }
}
