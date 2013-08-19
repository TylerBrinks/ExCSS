using System.Collections.Generic;

namespace ExCSS
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
