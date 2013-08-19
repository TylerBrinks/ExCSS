using System;
using ExCSS.Model;

namespace ExCSS
{
    public sealed class MediaRule : ConditionalRule
    {
        private readonly MediaTypes _media;

        public MediaRule(StyleSheetContext context) : base(context)
        {
            _media = new MediaTypes();
            RuleType = RuleType.Media;
        }

        public override string Condition
        {
            get { return _media.MediaType; }
            set { _media.MediaType = value; }
        }

        public MediaTypes Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return String.Format("@media {0} {{{1}{2}}}", _media.MediaType, Environment.NewLine, Declarations);
        }
    }
}
