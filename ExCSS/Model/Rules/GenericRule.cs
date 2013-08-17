
namespace ExCSS.Model
{
    sealed class GenericRule : RuleSet
    {
        private string _text;

        public GenericRule(StyleSheetContext context)
            : base(context)
        {
            
        }

        internal void SetText(string text)
        {
            _text = text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
