
namespace ExCSS.Model
{
    public abstract class Ruleset 
    {
        protected RuleType _type;
        public  StyleSheetContext Context;
        protected Ruleset _parentRule;
    

        internal Ruleset(StyleSheetContext context)
        {
  
            Context = context;
            _type = RuleType.Unknown;
        }

        public Ruleset ParentRule
        {
            get { return _parentRule; }
            internal set { _parentRule = value; }
        }

        public RuleType Type
        {
            get { return _type; }
        }
    }
}
