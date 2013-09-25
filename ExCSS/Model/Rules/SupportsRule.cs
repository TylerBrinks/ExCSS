using System;
using System.Linq;
using ExCSS.Model.Extensions;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class SupportsRule : ConditionalRule
    {
        private string _condition;

        public SupportsRule()
            : this(null)
        { }

        internal SupportsRule(StyleSheet context)
            : base(context)
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
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var prefix = friendlyFormat ? Environment.NewLine + "".Indent(friendlyFormat, indentation + 1) : "";
            var delcarationList = Declarations.Select(d => prefix + d.ToString(friendlyFormat, indentation + 1));
            var declarations = string.Join(" ", delcarationList);

            return "@supports " +
                _condition +
                "{" +
                declarations +
                "}".Indent(friendlyFormat, indentation);
        }
    }
}
