//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace ExCSS
//{
//    internal sealed class MarginRule : Rule //, IPageRule
//    {
//        private readonly string _name;

//        internal MarginRule(StylesheetParser parser, string name)
//            : base(RuleType.Page, parser)
//        {
//            _name = $"@{name}";
//            AppendChild(new StyleDeclaration(this));
//            Selector = new SimpleSelector(_name); // Parser.ParseSelector(name);
//        }

//        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
//        {
//            var rules = formatter.Block(Style);
//            writer.Write(formatter.Rule(_name, "", rules));

//            foreach (var margin in Margins) margin.ToCss(writer, formatter);
//        }

//        public string SelectorText
//        {
//            get => Selector.Text;
//            set => Selector = Parser.ParseSelector(value);
//        }


//        public ISelector Selector
//        {
//            get => Children.OfType<ISelector>().FirstOrDefault();
//            set => ReplaceSingle(Selector, value);
//        }

//        public StyleDeclaration Style => Children.OfType<StyleDeclaration>().FirstOrDefault();
//        public IEnumerable<MarginRule> Margins => Children.OfType<MarginRule>();
//    }
//}