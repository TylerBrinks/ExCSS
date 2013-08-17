
namespace ExCSS.Model
{
    public class Value
    {
        protected RuleValueType _type;
        protected string _text;
        static Value _inherited;

        internal Value()
        {
            _type = RuleValueType.Custom;
        }

        public static Value Inherit
        {
            get { return _inherited ?? (_inherited = new Value { _text = "inherit", _type = RuleValueType.Inherit }); }
        }

        public RuleValueType RuleValueType
        {
            get { return _type; }
        }

        public override string ToString()
        {
            return _text;
        }

        //UNITLESS in QUIRKSMODE:
        //  border-top-width
        //  border-right-width
        //  border-bottom-width
        //  border-left-width
        //  border-width
        //  bottom
        //  font-size
        //  height
        //  left
        //  letter-spacing
        //  margin
        //  margin-right
        //  margin-left
        //  margin-top
        //  margin-bottom
        //  padding
        //  padding-top
        //  padding-bottom
        //  padding-left
        //  padding-right
        //  right
        //  top
        //  width
        //  word-spacing

        //HASHLESS in QUIRKSMODE:
        //  background-color
        //  border-color
        //  border-top-color
        //  border-right-color
        //  border-bottom-color
        //  border-left-color
        //  color
    }
}
