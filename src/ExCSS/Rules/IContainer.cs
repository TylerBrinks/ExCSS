namespace ExCSS
{
    public interface IContainerRule : IConditionRule
    {
        string Name { get; set; }
        MediaList Media { get; }
  }
}