using System;
using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class KeyframesFactory : RuleFactory
    {
        public KeyframesFactory(StyleSheetContext context)
            : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> source)
        {
            throw new NotImplementedException();
        }
    }
}
