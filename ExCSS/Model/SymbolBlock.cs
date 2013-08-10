using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS keyword token.
    /// </summary>
    sealed class SymbolBlock : Block
    {
        #region Members

        string _value;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS keyword token.
        /// </summary>
        /// <param name="type">The exact type.</param>
        SymbolBlock(GrammarSegment type)
        {
            Type = type;
        }

        #endregion

        #region Static constructors

        /// <summary>
        /// Creates a new CSS keyword token for a function.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <returns>The created token.</returns>
        public static SymbolBlock Function(string name)
        {
            return new SymbolBlock(GrammarSegment.Function) { _value = name };
        }

        /// <summary>
        /// Creates a new CSS keyword token for an identifier.
        /// </summary>
        /// <param name="identifier">The name of the identifier.</param>
        /// <returns>The created token.</returns>
        public static SymbolBlock Ident(string identifier)
        {
            return new SymbolBlock(GrammarSegment.Ident) { _value = identifier };
        }

        /// <summary>
        /// Creates a new CSS keyword token for an at-keyword.
        /// </summary>
        /// <param name="name">The name of the @-rule.</param>
        /// <returns>The created token.</returns>
        public static SymbolBlock At(string name)
        {
            return new SymbolBlock(GrammarSegment.AtRule) { _value = name };
        }

        /// <summary>
        /// Creates a new CSS keyword token for a hash token.
        /// </summary>
        /// <param name="characters">The contained characters.</param>
        /// <returns>The created token.</returns>
        public static SymbolBlock Hash(string characters)
        {
            return new SymbolBlock(GrammarSegment.Hash) { _value = characters };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained data.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

        #endregion

        #region string representation

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToString()
        {
            switch (Type)
            {
                case GrammarSegment.Hash:
                    return "#" + Value;

                case GrammarSegment.AtRule:
                    return "@" + Value;
            }

            return Value;
        }

        #endregion
    }
}
