using System;
using System.Globalization;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS number token.
    /// </summary>
    sealed class NumericBlock : Block
    {
        #region Members

        string _data;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS number token.
        /// </summary>
        /// <param name="number">The number to contain.</param>
        public NumericBlock(string number)
        {
            _data = number;
            Type = GrammarSegment.Number;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained number.
        /// </summary>
        public Single Data
        {
            get { return Single.Parse(_data, CultureInfo.InvariantCulture); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToString()
        {
            return _data;
        }

        #endregion
    }
}
