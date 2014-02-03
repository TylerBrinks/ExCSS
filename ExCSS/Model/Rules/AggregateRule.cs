using System.Collections.Generic;
using ExCSS.Model;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public abstract class AggregateRule : RuleSet, /*IRuleContainer,*/ ISupportsRuleSets
    {
        protected AggregateRule()
        {
            RuleSets = new List<RuleSet>();
        }

        public List<RuleSet> RuleSets { get; private set; }
    }
}
