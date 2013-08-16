
namespace ExCSS.Model
{    
    public abstract class GroupingRule : Ruleset
    {
        protected GroupingRule(StyleSheetContext context)
            : base(context)
        {
            Rules = new RuleList();
        }

        public RuleList Rules { get; private set; }
    }
}
