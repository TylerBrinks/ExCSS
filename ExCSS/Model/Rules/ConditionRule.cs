

namespace ExCSS.Model
{
    public abstract class ConditionRule : AggregateRule
    {
        protected ConditionRule(StyleSheetContext context) : base(context)
        {
            
        }

        public virtual string ConditionText
        {
            get { return string.Empty; }
            set { }
        }
    }
}
