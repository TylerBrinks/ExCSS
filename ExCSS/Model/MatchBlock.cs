using System;

namespace ExCSS.Model
{
    /// <summary>
    /// The match token that contains part of a selector.
    /// </summary>
    sealed class MatchBlock : Block
    {
        #region Static instances

        readonly static MatchBlock include;
        readonly static MatchBlock dash;
        readonly static Block prefix;
        readonly static Block substring;
        readonly static Block suffix;
        readonly static Block not;

        #endregion

        #region ctor

        static MatchBlock()
        {
            include = new MatchBlock { _type = GrammarSegment.IncludeMatch };
            dash = new MatchBlock { _type = GrammarSegment.DashMatch };
            prefix = new MatchBlock { _type = GrammarSegment.PrefixMatch };
            substring = new MatchBlock { _type = GrammarSegment.SubstringMatch };
            suffix = new MatchBlock { _type = GrammarSegment.SuffixMatch };
            not = new MatchBlock { _type = GrammarSegment.NegationMatch };
        }

        /// <summary>
        /// Creates a new CSS match token.
        /// </summary>
        MatchBlock()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a new CSS include-match token.
        /// </summary>
        public static MatchBlock Include
        {
            get { return include; }
        }

        /// <summary>
        /// Gets a new CSS dash-match token.
        /// </summary>
        public static MatchBlock Dash
        {
            get { return dash; }
        }

        /// <summary>
        /// Gets a new CSS prefix-match token.
        /// </summary>
        public static Block Prefix
        {
            get { return prefix; }
        }

        /// <summary>
        /// Gets a new CSS substring-match token.
        /// </summary>
        public static Block Substring
        {
            get { return substring; }
        }

        /// <summary>
        /// Gets a new CSS suffix-match token.
        /// </summary>
        public static Block Suffix
        {
            get { return suffix; }
        }

        /// <summary>
        /// Gets a new CSS not-match token.
        /// </summary>
        public static Block Not
        {
            get { return not; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToValue()
        {
            switch (_type)
            {
                case GrammarSegment.SubstringMatch:
                    return "*=";

                case GrammarSegment.SuffixMatch:
                    return "$=";

                case GrammarSegment.PrefixMatch:
                    return "^=";

                case GrammarSegment.IncludeMatch:
                    return "~=";

                case GrammarSegment.DashMatch:
                    return "|=";

                case GrammarSegment.NegationMatch:
                    return "!=";
            }

            return String.Empty;
        }

        #endregion
    }
}
