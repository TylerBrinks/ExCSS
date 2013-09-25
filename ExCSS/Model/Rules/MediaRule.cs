using System;
using System.Linq;
using ExCSS.Model.Extensions;

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
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var prefix = friendlyFormat ? Environment.NewLine + "".Indent(friendlyFormat, indentation+1) : "";
            var declarationList = Declarations.Select(d => prefix + d.ToString(friendlyFormat, indentation+1));
            var declarations = string.Join(" ", declarationList);

            return "@media " +
                _media.MediaType +
                "{" +
                declarations +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
