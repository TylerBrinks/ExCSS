using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class SupportsRule : ConditionalRule
    {
        private string _condition;

        public SupportsRule() : this(null)
        {}

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
            return string.Format("@supports {0} {{{1}{2}}}", _condition, Environment.NewLine, Declarations);
        }
    }
}
