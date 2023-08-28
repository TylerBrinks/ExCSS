namespace ExCSS
{
    public struct CombinatorSelector
    {
        public string Delimiter { get; internal set; }
        public ISelector Selector { get; internal set; }
    }
}