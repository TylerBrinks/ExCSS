using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public abstract class AggregateRule : RuleSet, IRuleContainer
    {
        protected AggregateRule(StyleSheetContext context) : base(context)
        {
            Declarations = new List<RuleSet>();
        }

        public List<RuleSet> Declarations { get; private set; }
    }
}
