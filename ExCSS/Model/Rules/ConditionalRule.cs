
using ExCSS.Model;

namespace ExCSS
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
