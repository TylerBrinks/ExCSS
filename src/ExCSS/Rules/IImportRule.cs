namespace ExCSS
{
    public interface IImportRule : IRule
    {
        string Href { get; set; }
        MediaList Media { get; }
    }
}