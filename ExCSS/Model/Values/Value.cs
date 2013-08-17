
namespace ExCSS.Model
{
    public class Value
    {
        protected string Text;
        private static Value _inherited;

        internal Value()
        {
            RuleValueType = RuleValueType.Custom;
        }

        public static Value Inherit
        {
            get { return _inherited ?? (_inherited = new Value { Text = "inherit", RuleValueType = RuleValueType.Inherit }); }
        }

        public RuleValueType RuleValueType { get; internal set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
