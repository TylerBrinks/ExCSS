using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS string token.
    /// </summary>
    sealed class StringBlock : Block
    {
        #region Members

        string _data;
        bool _bad;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS string token.
        /// </summary>
        /// <param name="type">The exact type.</param>
        StringBlock(GrammarSegment type)
        {
            _type = type;
        }

        /// <summary>
        /// Creates a new CSS string token (plain string).
        /// </summary>
        /// <param name="data">The string data.</param>
        /// <param name="bad">If the string was bad (optional).</param>
        /// <returns>The created string token.</returns>
        public static StringBlock Plain(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.String) { _data = data, _bad = bad };
        }

        /// <summary>
        /// Creates a new CSS string token (URL string).
        /// </summary>
        /// <param name="data">The URL string data.</param>
        /// <param name="bad">If the URL was bad (optional).</param>
        /// <returns>The created URL string token.</returns>
        public static StringBlock Url(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.Url) { _data = data, _bad = bad };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained data.
        /// </summary>
        public string Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Gets if the data is bad.
        /// </summary>
        public bool IsBad
        {
            get { return _bad; }
        }

        #endregion

        #region string representation

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToValue()
        {
            if(_type == GrammarSegment.Url)
                return "url('" + _data + "')";

            return "'" + _data + "'";
        }

        #endregion
    }
}
