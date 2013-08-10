using System;
using System.Globalization;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS unit token.
    /// </summary>
    sealed class UnitBlock : Block
    {
        #region Members

        string _data;
        string _unit;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS unit token.
        /// </summary>
        /// <param name="type">The exact type.</param>
        UnitBlock(GrammarSegment type)
        {
            Type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained data.
        /// </summary>
        public Single Data
        {
            get { return Single.Parse(_data, CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Gets the contained unit.
        /// </summary>
        public string Unit
        {
            get { return _unit; }
        }

        #endregion

        #region Static Constructors

        /// <summary>
        /// Creates a new percentage unit token.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The created token.</returns>
        public static UnitBlock Percentage(string value)
        {
            return new UnitBlock(GrammarSegment.Percentage) { _data = value, _unit = "%" };
        }

        /// <summary>
        /// Creates a new dimension unit token.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dimension">The unit (dimension).</param>
        /// <returns>The created token.</returns>
        public static UnitBlock Dimension(string value, string dimension)
        {
            return new UnitBlock(GrammarSegment.Dimension) { _data = value, _unit = dimension };
        }

        #endregion

        public override string ToString()
        {
            return _data + _unit;
        }
    }
}
