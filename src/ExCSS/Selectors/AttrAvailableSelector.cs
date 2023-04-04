namespace ExCSS
{
    public sealed class AttrAvailableSelector : AttrSelectorBase
    {
        public AttrAvailableSelector(string attribute, string value)
            : base(attribute, value, $"[{attribute}]")
        {
        }
    }
}