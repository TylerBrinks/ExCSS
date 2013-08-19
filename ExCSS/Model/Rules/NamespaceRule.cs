using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class NamespaceRule : RuleSet
    {
        public NamespaceRule() : this(null)
        {
            
        }

        internal NamespaceRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Namespace;
        }

        public string Uri { get; set; }

        public string Prefix { get; set; }

        public override string ToString()
        {
            return string.Format("@namespace {0} '{1}';", Prefix, Uri);
        }
    }
}
