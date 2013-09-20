using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class MediaRule : ConditionalRule
    {
        private readonly MediaTypeList _media;

        public MediaRule() : this(null)
        {
            
        }

        internal MediaRule(StyleSheet context) : base(context)
        {
            _media = new MediaTypeList();
            RuleType = RuleType.Media;
        }

        public override string Condition
        {
            get { return _media.MediaType; }
            set { _media.MediaType = value; }
        }

        public MediaTypeList Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            var declarations = string.Join(" ", Declarations);

            return string.Format("@media {0}{{{1}}}", _media.MediaType, declarations);
        }
    }
}
