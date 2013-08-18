using System.Collections.Generic;

namespace ExCSS.Model.Rules
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}