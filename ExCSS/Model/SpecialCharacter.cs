using System;

namespace ExCSS.Model
{
    /// <summary>
    /// The special character token that contains a special character such as a colon.
    /// </summary>
    sealed class SpecialCharacter : CharacterBlock
    {
        #region Static instances

        static readonly SpecialCharacter colon;
        static readonly SpecialCharacter comma;
        static readonly SpecialCharacter semicolon;
        static readonly SpecialCharacter whitespace;

        #endregion

        #region ctor

        static SpecialCharacter()
        {
            colon = new SpecialCharacter(Specification.COLON, GrammarSegment.Colon);
            comma = new SpecialCharacter(Specification.COMMA, GrammarSegment.Comma);
            semicolon = new SpecialCharacter(Specification.SC, GrammarSegment.Semicolon);
            whitespace = new SpecialCharacter(Specification.SPACE, GrammarSegment.Whitespace);
        }

        /// <summary>
        /// Creates a new special character token.
        /// </summary>
        /// <param name="c">The character to contain.</param>
        /// <param name="type">The actual token type.</param>
        SpecialCharacter(Char c, GrammarSegment type)
            : base(c)
        {
            _type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a colon token.
        /// </summary>
        public static SpecialCharacter Colon
        {
            get { return colon; }
        }

        /// <summary>
        /// Gets a new comma token.
        /// </summary>
        public static SpecialCharacter Comma
        {
            get { return comma; }
        }

        /// <summary>
        /// Gets a new comma token.
        /// </summary>
        public static SpecialCharacter Semicolon
        {
            get { return semicolon; }
        }

        /// <summary>
        /// Gets a new comma token.
        /// </summary>
        public static SpecialCharacter Whitespace
        {
            get { return whitespace; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToValue()
        {
            return Value.ToString();
        }

        #endregion
    }
}
