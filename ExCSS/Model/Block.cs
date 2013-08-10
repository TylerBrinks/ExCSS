using System;
using System.Diagnostics;

namespace ExCSS.Model
{
    //[DebuggerStepThrough]
    abstract class Block
    {
        #region Members

        protected GrammarSegment _type;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        public GrammarSegment Type
        {
            get { return _type; }
        }

        #endregion

        #region Factory

        /// <summary>
        /// Gets the column token.
        /// </summary>
        public static PipeBlock Column
        {
            get { return PipeBlock.Token; }
        }

        /// <summary>
        /// Creates a new CSS delimiter token.
        /// </summary>
        /// <param name="c">The delim char.</param>
        /// <returns>The created token.</returns>
        //[DebuggerStepThrough]
        public static DelimBlock Delim(Char c)
        {
            return new DelimBlock(c);
        }

        /// <summary>
        /// Creates a new CSS number token.
        /// </summary>
        /// <param name="value">The single precision number.</param>
        /// <returns>The created token.</returns>
        //[DebuggerStepThrough]
        public static NumericBlock Number(string value)
        {
            return new NumericBlock(value);
        }

        /// <summary>
        /// Creates a new CSS range token.
        /// </summary>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        /// <returns>The created token.</returns>
        //[DebuggerStepThrough]
        public static RangeBlock Range(string start, string end)
        {
            return new RangeBlock().SetRange(start, end);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public abstract string ToValue();

        #endregion
    }
}
