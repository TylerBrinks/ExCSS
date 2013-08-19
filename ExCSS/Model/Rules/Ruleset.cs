
// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public abstract class RuleSet 
    {
        internal StyleSheetContext Context;

        internal RuleSet(StyleSheetContext context)
        {
            Context = context;
            RuleType = RuleType.Unknown;
        }

        public RuleType RuleType { get; set; }
    }
}
