using System.Collections.Generic;

namespace ExCSS
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}