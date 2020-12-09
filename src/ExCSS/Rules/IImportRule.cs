namespace ExCSS
{
    public interface IImportRule
    {
        string Href { get; set; }
        MediaList Media { get; }
    }
}