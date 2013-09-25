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
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.IsNullOrEmpty(Prefix)
                 ? string.Format("@namespace {0};{1}", Uri, Environment.NewLine)
                 : string.Format("@namespace {0} {1};{2}", Prefix, Uri, Environment.NewLine);
        }
    }
}
