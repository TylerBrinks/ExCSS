namespace ExCSS
{
    internal sealed class LastTypeSelector : ChildSelector
    {
        public LastTypeSelector()
            : base(PseudoClassNames.NthLastOfType)
        {
        }
    }
}