using System;
using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class UnknownAtRuleFactory : RuleFactory
    {
        public UnknownAtRuleFactory(StyleSheetContext context)
            : base(context)
        {
        }
        
        public override void Parse(IEnumerator<Block> source)
        {
            throw new NotImplementedException();
        }
    }
}