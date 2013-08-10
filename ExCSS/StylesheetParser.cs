//using Antlr.Runtime;
//using Antlr.Runtime.Tree;
//using ExCSS.Model;

//namespace ExCSS
//{
//    /// <summary>
//    /// Stylesheet Parser reads CSS rules compatible up to CSS v3.
//    /// </summary>
//    public class StylesheetParser
//    {
//        /// <summary>
//        /// Parses the specified content.
//        /// </summary>
//        /// <param name="content">The content.</param>
//        /// <returns></returns>
//        public Stylesheet Parse(string content)
//        {
//            var stream = new ANTLRStringStream(content);

//            var lexer = new ExCSSLexer(stream);
//            var tokens = new CommonTokenStream(lexer);
//            var parser = new ExCSSParser(tokens);

//            var tree = parser.parse().Tree;

//            return BuildStylesheet(tree);
//        }

//        private static Stylesheet BuildStylesheet(ITree tree)
//        {
//            var stylesheet = new Stylesheet();
//            stylesheet.Build(tree);

//            return stylesheet;
//        }
//    }
//}