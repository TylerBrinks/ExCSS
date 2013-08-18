using System.Collections.Generic;
using ExCSS.Model.Rules;

namespace ExCSS.Model
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
