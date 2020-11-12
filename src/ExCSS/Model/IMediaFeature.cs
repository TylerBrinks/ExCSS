namespace ExCSS
{
    public interface IMediaFeature : IStylesheetNode
    {
        string Name { get; }
        //bool IsMinimum { get; }
        //bool IsMaximum { get; }
        string Value { get; }
        bool HasValue { get; }
    }
}