namespace ExCSS
{
    /// <summary>The <c>:only-child</c> structural pseudo-class.</summary>
    public sealed class OnlyChildSelector : SelectorBase
    {
        public OnlyChildSelector()
            : base(Priority.OneClass, $"{PseudoClassNames.Separator}{PseudoClassNames.OnlyChild}")
        {
        }
    }
}
