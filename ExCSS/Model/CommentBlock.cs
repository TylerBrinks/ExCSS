using System;

namespace ExCSS.Model
{
    sealed class CommentBlock : Block
    {
        #region Static instances

        readonly static CommentBlock open;
        readonly static CommentBlock close;

        #endregion

        #region ctor

        static CommentBlock()
        {
            open = new CommentBlock { Type = GrammarSegment.CommentOpen };
            close = new CommentBlock { Type = GrammarSegment.CommentClose };
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        CommentBlock()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a new CSS open comment token.
        /// </summary>
        public static CommentBlock Open
        {
            get { return open; }
        }

        /// <summary>
        /// Gets a new CSS close comment token.
        /// </summary>
        public static CommentBlock Close
        {
            get { return close; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string which represents the original value.
        /// </summary>
        /// <returns>The original value.</returns>
        public override string ToString()
        {
            return Type == GrammarSegment.CommentOpen ? "<!--" : "-->";
        }

        #endregion
    }
}
