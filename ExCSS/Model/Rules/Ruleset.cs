
namespace ExCSS.Model
{
    public abstract class RuleSet 
    {
        public  StyleSheetContext Context;

        internal RuleSet(StyleSheetContext context)
        {
  
            Context = context;
            RuleType = RuleType.Unknown;
        }

        public RuleType RuleType { get; set; }
    }
}
