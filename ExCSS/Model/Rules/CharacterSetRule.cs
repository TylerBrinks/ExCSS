using System;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class CharacterSetRule : RuleSet
    {
        public CharacterSetRule() : this(null)
        {}

        internal CharacterSetRule(StyleSheet context) : base(context)
        {
            RuleType = RuleType.Charset;
        }

        public string Encoding { get; internal set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.Format("@charset '{0}';{1}", Encoding, Environment.NewLine);
        }
    }
}
