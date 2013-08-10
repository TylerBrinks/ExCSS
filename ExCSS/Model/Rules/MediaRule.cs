using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS @media rule.
    /// </summary>
    //[DOM("MediaRule")]
    public sealed class MediaRule : ConditionRule
    {
        #region Constants

        internal const string RuleName = "media";

        #endregion

        #region Members

        MediaList _media;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS @media rule.
        /// </summary>
        internal MediaRule()
        {
            _media = new MediaList();
            _type = RuleType.Media;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text of the media condition.
        /// </summary>
        //[DOM("conditionText")]
        public override string ConditionText
        {
            get { return _media.MediaText; }
            set { _media.MediaText = value; }
        }

        /// <summary>
        /// Gets a list of media types for this rule.
        /// </summary>
        //[DOM("media")]
        public MediaList Media
        {
            get { return _media; }
        }

        #endregion

       public override string ToString()
        {
            return String.Format("@media {0} {{{1}{2}}}", _media.MediaText, Environment.NewLine, Rules);
        }

    }
}
