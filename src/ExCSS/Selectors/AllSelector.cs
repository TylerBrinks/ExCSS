namespace ExCSS
{
    public sealed class AllSelector : SelectorBase
    {
        public static AllSelector Create()
        {
            return new AllSelector();
        }

        private AllSelector() : base(Priority.Zero, "*")
        {
        }
    }
}