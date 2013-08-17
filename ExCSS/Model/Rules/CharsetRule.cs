using System;

namespace ExCSS.Model.Rules
{
    public sealed class CharsetRule : RuleSet
    {
        internal CharsetRule(StyleSheetContext context) : base(context)
        {
            RuleType = RuleType.Charset;
        }

        public string Encoding { get; internal set; }

        public override string ToString()
        {
            return String.Format("@charset '{0}';", Encoding);
        }
    }
}
