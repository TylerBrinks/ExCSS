using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}