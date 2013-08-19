using System.Collections.Generic;
using ExCSS.Model.Extensions;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal class AtRuleFactory : RuleFactory
    {
        public AtRuleFactory(StyleSheetContext context) : base( context)
        {}

        public override void Parse(IEnumerator<Block> reader)
        {
            var name = ((SymbolBlock)reader.Current).Value;
            
            reader.SkipToNextNonWhitespace();
            
            var factory = GetAtRuleFactory(name);

            factory.Parse(reader);
        }

        private IRuleFactory GetAtRuleFactory(string name)
        {
            switch (name)
            {
                case RuleTypes.Media:
                    return  new MediaRuleFactory(Context);

                case RuleTypes.Page:
                    return  new PageRuleFactory(Context);

                case RuleTypes.Import:
                    return  new ImportRuleFactory(Context);

                case RuleTypes.FontFace:
                    return  new FontFaceFactory(Context);

                case RuleTypes.CharacterSet:
                    return  new CharacterSetFactory(Context);

                case RuleTypes.Namespace:
                    return  new NamespaceFactory(Context);

                case RuleTypes.Supports:
                    return  new SupportFactory(Context);

                case RuleTypes.Keyframes:
                    return  new KeyframesFactory(Context);

                default:
                    return  new UnknownAtRuleFactory(name, Context);
            }
        }
    }
}