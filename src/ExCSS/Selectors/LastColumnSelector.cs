namespace ExCSS
{
    public sealed class LastColumnSelector : ChildSelector
    {
        public LastColumnSelector()
            : base(PseudoClassNames.NthLastColumn)
        {
        }
    }
}