namespace ExCSS

{
    public sealed class FirstChildSelector : ChildSelector
    {
        public FirstChildSelector()
            : base(PseudoClassNames.NthChild)
        {
        }
    }
}