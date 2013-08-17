using System.Collections.Generic;
using ExCSS.Model.Rules;

namespace ExCSS.Model
{
    public abstract class AggregateRule : RuleSet, IRuleContainer
    {
        protected AggregateRule(StyleSheetContext context) : base(context)
        {
            Rules = new List<RuleSet>();
        }

        public List<RuleSet> Rules { get; private set; }
    }
}
