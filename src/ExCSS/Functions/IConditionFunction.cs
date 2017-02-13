namespace ExCSS
{
    public interface IConditionFunction : IStylesheetNode
    {
        bool Check();
    }
}