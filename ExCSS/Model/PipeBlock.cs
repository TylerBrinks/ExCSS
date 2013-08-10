using System;

namespace ExCSS.Model
{
    /// <summary>
    /// The column token that contains a column (||).
    /// </summary>
    sealed class PipeBlock : Block
    {
        #region Static instance

        readonly static PipeBlock token;

        #endregion

        #region ctor

        static PipeBlock()
        {
            token = new PipeBlock();
        }

        /// <summary>
        /// Creates a new CSS column token.
        /// </summary>
        PipeBlock()
        {
            _type = GrammarSegment.Column;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the token.
        /// </summary>
        public static PipeBlock Token
        {
            get { return token; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToValue()
        {
            return "||";
        }

        #endregion
    }
}
