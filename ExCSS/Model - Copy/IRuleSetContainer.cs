using System.Collections.Generic;

namespace ExCSS.Model 
{
	public interface IRuleSetContainer 
    {
		List<RuleSet> RuleSets { get; }
	}
}