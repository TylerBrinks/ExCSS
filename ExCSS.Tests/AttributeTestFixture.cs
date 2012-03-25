using NUnit.Framework;

namespace ExCSS.Tests
{
	[TestFixture]
    public partial class AttributeTestFixture
    {
        [Test]
        public void Content_With_Empty_Single_Quote_Strings_Is_Parsed()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{content: '  ' }");
            var expression = style.RuleSets[0].Declarations[0].Expression;

            Assert.AreEqual("'  '", expression.Terms[0].ToString());
        }

        [Test]
        public void Content_With_Empty_Double_Quote_Strings_Is_Parsed()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{content: \"  \" }");
            var expression = style.RuleSets[0].Declarations[0].Expression;

            Assert.AreEqual("\"  \"", expression.Terms[0].ToString());
        }

        [Test]
        public void Terms_With_Vendor_Functions_Are_Parsed()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{background-image: -moz-linear-gradient(top,#CCC,#ddd) }");
            var expression = style.RuleSets[0].Declarations[0].Expression;

            Assert.AreEqual("-moz-linear-gradient(top, #CCC, #DDD)", expression.Terms[0].ToString());
        }

        [Test]
        public void Terms_With_Functions_Are_Parsed()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{clip: rect(0px,60px,200px,0px) }");
            var expression = style.RuleSets[0].Declarations[0].Expression;

            Assert.AreEqual(1, expression.Terms.Count);
            Assert.AreEqual("rect(0px, 60px, 200px, 0px)", expression.Terms[0].ToString());
        }

        [Test]
        public void Terms_With_Size_And_Height_Shorthand_Are_Parsed()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{font-size: 12px/20px }");
            var expression = style.RuleSets[0].Declarations[0].Expression;

            Assert.AreEqual("12px/20px", expression.Terms[0].ToString());
        }

        [Test]
        public void Empty_Terms_Are_Ignored()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{ font-size: 10px; ; }");

            Assert.AreEqual(1, style.RuleSets[0].Declarations.Count);
        }
    }
}
