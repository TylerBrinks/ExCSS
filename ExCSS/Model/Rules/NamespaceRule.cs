using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class NamespaceRule : RuleSet
    {
        public NamespaceRule() : this(null)
        {}

        internal NamespaceRule(StyleSheet context) : base(context)
        {
            RuleType = RuleType.Namespace;
        }

        public string Uri { get; set; }

        public string Prefix { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Prefix) 
                ? string.Format("@namespace {0};", Uri) 
                : string.Format("@namespace {0} {1};", Prefix, Uri);
        }
    }
}
