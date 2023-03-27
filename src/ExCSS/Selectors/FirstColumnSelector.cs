namespace ExCSS
{
    public sealed class FirstColumnSelector : ChildSelector
    {
        public FirstColumnSelector()
            : base(PseudoClassNames.NthColumn)
        {
        }
    }
}