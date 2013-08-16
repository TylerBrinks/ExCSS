using System;

namespace ExCSS.Model
{
    public sealed class ImportRule : Ruleset
    {
        internal const string RuleName = "import";

        private string _href;
        private readonly MediaQueries _media;

        internal ImportRule(StyleSheetContext context)
            : base(context)
        {
            _media = new MediaQueries();
            _type = RuleType.Import;
        }
      
        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }

        public MediaQueries Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return String.Format("@import url('{0}') {1};", _href, _media.MediaText);
        }
    }
}
