using System;

namespace ExCSS.Model
{
    public sealed class NamespaceRule : RuleSet
    {
        internal NamespaceRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Namespace;
        }

        public string Uri { get; set; }

        public string Prefix { get; set; }

        public override string ToString()
        {
            return String.Format("@namespace {0} '{1}';", Prefix, Uri);
        }
    }
}
