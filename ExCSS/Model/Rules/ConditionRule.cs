

namespace ExCSS.Model
{
    public abstract class ConditionRule : GroupingRule
    {
        public ConditionRule(StyleSheetContext context)
            : base(context)
        {
            
        }
        public virtual string ConditionText
        {
            get { return string.Empty; }
            set { }
        }
    }
}
