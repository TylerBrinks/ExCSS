
namespace ExCSS.Model
{
    sealed class StringBlock : Block
    {
        private string _value;
        private bool _bad;

        StringBlock(GrammarSegment type)
        {
            Type = type;
        }
         
        public static StringBlock Plain(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.String) { _value = data, _bad = bad };
        }

        public static StringBlock Url(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.Url) { _value = data, _bad = bad };
        }

        public string Value
        {
            get { return _value; }
        }

        public bool IsBad
        {
            get { return _bad; }
        }

        public override string ToString()
        {
            if (Type == GrammarSegment.Url)
            {
                return "url('" + _value + "')";
            }

            return "'" + _value + "'";
        }
    }
}
