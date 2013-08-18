using System;

namespace ExCSS.Model
{
    public sealed class SupportsRule : ConditionalRule
    {
        private string _condition;

        internal SupportsRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Supports;
            _condition = string.Empty;
        }

        public override string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        public override string ToString()
        {
            return String.Format("@supports {0} {{{1}{2}}}", _condition, Environment.NewLine, Declarations);
        }
    }
}
