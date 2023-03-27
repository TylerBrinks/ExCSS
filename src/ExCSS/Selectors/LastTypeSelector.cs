namespace ExCSS
{
    public sealed class LastTypeSelector : ChildSelector
    {
        public LastTypeSelector()
            : base(PseudoClassNames.NthLastOfType)
        {
        }
    }
}