namespace ExCSS
{
    public interface IMediaRule : IConditionRule
    {
        MediaList Media { get; }
    }
}