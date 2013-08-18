using System;

namespace ExCSS.Model
{
    public sealed class ImportRule : RuleSet
    {
        private string _href;
        private readonly MediaTypes _media;

        internal ImportRule(StyleSheetContext context) : base(context)
        {
            _media = new MediaTypes();
            RuleType = RuleType.Import;
        }
      
        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }

        public MediaTypes Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return String.Format("@import url('{0}') {1};", _href, _media.MediaType);
        }
    }
}
