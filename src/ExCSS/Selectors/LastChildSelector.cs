
namespace ExCSS
{
    public sealed class LastChildSelector : ChildSelector
    {
        public LastChildSelector()
            : base(PseudoClassNames.NthLastChild)
        {
        }
    }
}