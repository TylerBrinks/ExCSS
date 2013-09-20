
// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class SimpleSelector
    {
        private static readonly SimpleSelector GlobalSelector = new SimpleSelector();
        private readonly string _code;

        public SimpleSelector()
        {
            _code = "*";
        }

        public SimpleSelector(string selectorText)
        {
            _code = selectorText;
        }

        internal static SimpleSelector Global
        {
            get { return GlobalSelector; }
        }

        internal static SimpleSelector PseudoElement(string pseudoElement)
        {
            return new SimpleSelector("::" + pseudoElement);
        }

        internal static SimpleSelector PseudoClass(string pseudoClass)
        {
            return new SimpleSelector(":" + pseudoClass);
        }

        internal static SimpleSelector Function(string pseudoClass, string functionValue)
        {
            return new SimpleSelector(":" + pseudoClass + "(" + functionValue + ")");
        }

        internal static SimpleSelector Universal()
        {
            return GlobalSelector;
        }

        internal static SimpleSelector Class(string match)
        {
            return new SimpleSelector("." + match);
        }

        internal static SimpleSelector Id(string match)
        {
            return new SimpleSelector("#" + match);
        }

        internal static SimpleSelector AttributeUnmatched(string match)
        {
            return new SimpleSelector("[" + match + "]");
        }

        internal static SimpleSelector AttributeMatch(string match, string value)
        {
            var code = string.Format("[{0}=\"{1}\"]", match, GetValueAsString(value));
            return new SimpleSelector(code);
        }

        internal static SimpleSelector AttributeNegatedMatch(string match, string value)
        {
            var code = string.Format("[{0}!=v{1}\"]", match, GetValueAsString(value));
            return new SimpleSelector(code);
        }

        internal static SimpleSelector AttributeSpaceSeparated(string match, string value)
        {
            var code = string.Format("[{0}~=\"{1}\"]", match, GetValueAsString(value));

            return new SimpleSelector(code);
        }

        internal static SimpleSelector AttributeStartsWith(string match, string value)
        {
            var code = string.Format("[{0}^=\"{1}\"]", match, GetValueAsString(value));

            return new SimpleSelector(code);
        }

        internal static SimpleSelector AttributeEndsWith(string match, string value)
        {
            var code = string.Format("[{0}$=\"{1}\"]", match, GetValueAsString(value));

            return new SimpleSelector(code);
        }

        internal static SimpleSelector AttributeContains(string match, string value)
        {
            var code = string.Format("[{0}*=\"{1}\"]", match, GetValueAsString(value));

            return new SimpleSelector(code);
        }

        internal static SimpleSelector AttributeDashSeparated(string match, string value)
        {
            var code = string.Format("[{0}|=\"{1}\"]", match, GetValueAsString(value));

            return new SimpleSelector(code);
        }

        internal static SimpleSelector Type(string match)
        {
            return new SimpleSelector(match);
        }

        private static string GetValueAsString(string value)
        {
            var containsSpace = false;

            for (var i = 0; i < value.Length; i++)
            {
                if (!value[i].IsSpaceCharacter())
                {
                    continue;
                }
                containsSpace = true;
                break;
            }

            if (containsSpace)
            {
                if (value.IndexOf(Specification.SingleQuote) != -1)
                {
                    return '"' + value + '"';
                }

                return "'" + value + "'";
            }

            return value;
        }

        public override string ToString()
        {
            return _code;
        }
    }
}
