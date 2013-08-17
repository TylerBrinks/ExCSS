using System;

namespace ExCSS.Model
{

    internal class SimpleSelector : Selector
    {
        private static readonly SimpleSelector AllSelector = new SimpleSelector();
        private readonly int _specifity;
        private readonly string _code;

        public SimpleSelector()
        {
            _code = "*";
            _specifity = 0;
        }

        public SimpleSelector(string match)
        {
            _specifity = 1;
            _code = match;
        }

        public SimpleSelector(int specifify, string code)
        {
            _specifity = specifify;
            _code = code;
        }

        public static Selector All
        {
            get { return AllSelector; }
        }

        public override int Specifity
        {
            get { return _specifity; }
        }

        public static SimpleSelector PseudoElement(string pseudoElement)
        {
            return new SimpleSelector(1, "::" + pseudoElement);
        }

        public static SimpleSelector PseudoClass(string pseudoClass)
        {
            return new SimpleSelector(10, ":" + pseudoClass);
        }

        public static SimpleSelector Universal()
        {
            return AllSelector;
        }

        public static SimpleSelector Class(string match)
        {
            return new SimpleSelector(10, "." + match);
        }

        public static SimpleSelector Id(string match)
        {
            return new SimpleSelector(100, "#" + match);
        }

        public static SimpleSelector AttrAvailable(string match)
        {
            return new SimpleSelector(10, "[" + match + "]");
        }

        public static SimpleSelector AttrMatch(string match, string value)
        {
            var code = String.Format("[{0}={1}]", match, GetValueAsString(value));
            return new SimpleSelector(10, code);
        }

        public static SimpleSelector AttrNotMatch(string match, string value)
        {
            var code = String.Format("[{0}!={1}]", match, GetValueAsString(value));
            return new SimpleSelector(10, code);
        }

        public static SimpleSelector AttrList(string match, string value)
        {
            var code = string.Format("[{0}~={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        public static SimpleSelector AttrBegins(string match, string value)
        {
            var code = String.Format("[{0}^={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        public static SimpleSelector AttrEnds(string match, string value)
        {
            var code = String.Format("[{0}$={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        public static SimpleSelector AttrContains(string match, string value)
        {
            var code = String.Format("[{0}*={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        public static SimpleSelector AttrHyphen(string match, string value)
        {
            var code = String.Format("[{0}|={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        public static SimpleSelector Type(string match)
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
