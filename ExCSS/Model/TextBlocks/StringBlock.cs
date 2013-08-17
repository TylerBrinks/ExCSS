
namespace ExCSS.Model
{
    sealed class StringBlock : Block
    {
        StringBlock(GrammarSegment type)
        {
            Type = type;
        }
         
        public static StringBlock Plain(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.String) { Value = data, IsBad = bad };
        }

        public static StringBlock Url(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.Url) { Value = data, IsBad = bad };
        }

        public string Value { get; private set; }

        public bool IsBad { get; private set; }

        public override string ToString()
        {
            if (Type == GrammarSegment.Url)
            {
                return "url('" + Value + "')";
            }

            return "'" + Value + "'";
        }
    }
}
