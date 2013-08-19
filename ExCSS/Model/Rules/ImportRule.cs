using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class ImportRule : RuleSet
    {
        private string _href;
        private readonly MediaTypeList _media;

        public ImportRule() : this(null)
        {
            
        }

        internal ImportRule(StyleSheetContext context) : base(context)
        {
            _media = new MediaTypeList();
            RuleType = RuleType.Import;
        }
      
        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }

        public MediaTypeList Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return _media.Count > 0 
                ? string.Format("@import url('{0}') {1};", _href, _media) 
                : string.Format("@import url('{0}');", _href);
        }
    }
}
