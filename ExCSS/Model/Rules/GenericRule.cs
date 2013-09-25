
// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class GenericRule : RuleSet
    {
        private string _text;

        public GenericRule() : this(null)
        { }
        
        internal GenericRule(StyleSheet context) : base(context)
        { }

        internal void SetText(string text)
        {
            _text = text;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return _text;
        }
    }
}
