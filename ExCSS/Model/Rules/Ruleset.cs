
namespace ExCSS.Model
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
