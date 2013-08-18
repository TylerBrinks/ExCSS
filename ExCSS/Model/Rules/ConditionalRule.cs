

namespace ExCSS.Model
{
    public abstract class ConditionalRule : AggregateRule
    {
        protected ConditionalRule(StyleSheetContext context) : base(context)
        {}

        public virtual string Condition
        {
            get { return string.Empty; }
            set { }
        }
    }
}
