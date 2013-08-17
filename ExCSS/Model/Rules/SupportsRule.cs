using System;

namespace ExCSS.Model
{
    public sealed class SupportsRule : ConditionRule
    {
        private string _conditionText;

        internal SupportsRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Supports;
            _conditionText = string.Empty;
        }

        public override string ConditionText
        {
            get { return _conditionText; }
            set { _conditionText = value; }
        }

        public override string ToString()
        {
            return String.Format("@supports {0} {{{1}{2}}}", _conditionText, Environment.NewLine, Rules);
        }
    }
}
