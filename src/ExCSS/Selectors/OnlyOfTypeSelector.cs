namespace ExCSS
{
    /// <summary>The <c>:only-of-type</c> structural pseudo-class.</summary>
    public sealed class OnlyOfTypeSelector : SelectorBase
    {
        public OnlyOfTypeSelector()
            : base(Priority.OneClass, $"{PseudoClassNames.Separator}{PseudoClassNames.OnlyType}")
        {
        }
    }
}
