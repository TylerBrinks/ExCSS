using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a simple selector (either a type selector,
    /// universal selector, attribute selector, class selector,
    /// id selector or pseudo-class).
    /// </summary>
    internal class SimpleSelector : Selector
    {
        private static readonly SimpleSelector _all = new SimpleSelector();

        //Predicate<Element> _matches;
        private readonly Int32 _specifity;
        private string _code;

        
        public SimpleSelector()
        {
            //_matches = _ => true;
            _code = "*";
            _specifity = 0;
        }
       
        public SimpleSelector(string match)
        {
            //_matches = _ => _.TagName.Equals(match, StringComparison.OrdinalIgnoreCase);
            _specifity = 1;
            _code = match;
        }

        public SimpleSelector(int specifify, string code)
        {
            //_matches = matches;
            _specifity = specifify;
            _code = code;
        }

        public static Selector All
        {
            get { return _all; }
        }
     
        public override int Specifity
        {
            get { return _specifity; }
        }

        #region Static constructors

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
            return _all;
        }

        public static SimpleSelector Class(string match)
        {
            return new SimpleSelector( 10, "." + match);
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

        /// <summary>
        /// Creates a new attribute matches the begin selector.
        /// </summary>
        /// <param name="match">The attribute that has to be available.</param>
        /// <param name="value">The begin of the value of the attribute.</param>
        /// <returns>The new selector.</returns>
        public static SimpleSelector AttrBegins(string match, string value)
        {
            var code = String.Format("[{0}^={1}]", match, GetValueAsString(value));

            if (String.IsNullOrEmpty(value))
            {
                return new SimpleSelector(10, code);
            }

            return new SimpleSelector(10, code);
        }

        /// <summary>
        /// Creates a new attribute matches the end selector.
        /// </summary>
        /// <param name="match">The attribute that has to be available.</param>
        /// <param name="value">The end of the value of the attribute.</param>
        /// <returns>The new selector.</returns>
        public static SimpleSelector AttrEnds(string match, string value)
        {
            var code = String.Format("[{0}$={1}]", match, GetValueAsString(value));

            if (String.IsNullOrEmpty(value))
                return new SimpleSelector(10, code);

            return new SimpleSelector(10, code);
        }

        /// <summary>
        /// Creates a new attribute contains selector.
        /// </summary>
        /// <param name="match">The attribute that has to be available.</param>
        /// <param name="value">The value that has to be contained in the value of the attribute.</param>
        /// <returns>The new selector.</returns>
        public static SimpleSelector AttrContains(string match, string value)
        {
            var code = String.Format("[{0}*={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        /// <summary>
        /// Creates a new attribute matches hyphen separated list selector.
        /// </summary>
        /// <param name="match">The attribute that has to be available.</param>
        /// <param name="value">The value that has to be a hyphen separated list entry of the attribute.</param>
        /// <returns>The new selector.</returns>
        public static SimpleSelector AttrHyphen(string match, string value)
        {
            var code = String.Format("[{0}|={1}]", match, GetValueAsString(value));

            return new SimpleSelector(10, code);
        }

        /// <summary>
        /// Creates a new type selector.
        /// </summary>
        /// <param name="match">The type to match (the tagname).</param>
        /// <returns>The new selector.</returns>
        public static SimpleSelector Type(string match)
        {
            return new SimpleSelector(match);
        }

        #endregion

        #region Helpers

        static string GetValueAsString(string value)
        {
            var containsSpace = false;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].IsSpaceCharacter())
                {
                    containsSpace = true;
                    break;
                }
            }

            if (containsSpace)
            {
                if (value.IndexOf(Specification.SQ) != -1)
                    return '"' + value + '"';

                return "'" + value + "'";
            }

            return value;
        }

        #endregion

        //public override bool Match(Element element)
        //{
        //    return _matches(element);
        //}

        public override string ToString()
        {
            return _code;
        }
    }
}
