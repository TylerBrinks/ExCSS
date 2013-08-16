using System;
using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    class FontFaceFactory:RuleFactory
    {
        public FontFaceFactory(StyleSheetContext context)
            : base(context)
        {
        }

        public override void Parse(IEnumerator<Block> source)
        {
            throw new NotImplementedException();
        }
    }
}
