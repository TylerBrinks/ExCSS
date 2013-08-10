using System;

namespace ExCSS.Model
{
    public sealed class ImportRule : Ruleset
    {
        internal const string RuleName = "import";

        string _href;
        MediaList _media;
        StyleSheet _styleSheet;
        
        internal ImportRule()
        {
            _media = new MediaList();
            _type = RuleType.Import;
        }

      
        public string Href
        {
            get { return _href; }
            internal set { _href = value; }
        }

       
        public MediaList Media
        {
            get { return _media; }
        }

        public StyleSheet StyleSheet
        {
            get { return _styleSheet; }
            internal set { _styleSheet = value; }
        }

      

       public override string ToString()
        {
            return String.Format("@import url('{0}') {1};", _href, _media.MediaText);
        }
    }
}
