namespace ExCSS
{
    public interface ISupportsRule : IConditionRule
    {
        IConditionFunction Condition { get; }
    }
}