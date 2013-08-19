using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class CharacterSetRule : RuleSet
    {
        public CharacterSetRule() : this(null)
        {}

        internal CharacterSetRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Charset;
        }

        public string Encoding { get; internal set; }

        public override string ToString()
        {
            return string.Format("@charset '{0}';", Encoding);
        }
    }
}
