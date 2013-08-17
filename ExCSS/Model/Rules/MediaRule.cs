using System;

namespace ExCSS.Model.Rules
{
    public sealed class MediaRule : ConditionRule
    {
        private readonly MediaQueries _media;

        public MediaRule(StyleSheetContext context) : base(context)
        {
            _media = new MediaQueries();
            RuleType = RuleType.Media;
        }

        public override string ConditionText
        {
            get { return _media.MediaText; }
            set { _media.MediaText = value; }
        }

        public MediaQueries Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return String.Format("@media {0} {{{1}{2}}}", _media.MediaText, Environment.NewLine, Rules);
        }
    }
}
