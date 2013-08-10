
namespace ExCSS.Model
{
    public abstract class Ruleset 
    {
        protected RuleType _type;
        protected StyleSheet _parent;
        protected Ruleset _parentRule;

        internal Ruleset()
        {
            _type = RuleType.Unknown;
        }

        public Ruleset ParentRule
        {
            get { return _parentRule; }
            internal set { _parentRule = value; }
        }

        //public StyleSheet ParentStyleSheet
        //{
        //    get { return _parent; }
        //    internal set { _parent = value; }
        //}

        public RuleType Type
        {
            get { return _type; }
        }
    }
}
