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

        [Test]
        public void Zero_Values_Before_Decimals_Are_Parsed()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse("*{ background-position: 0 1.1em; }");

            Assert.AreEqual(1, style.RuleSets[0].Declarations.Count);
        }

        [Test]
        public void Auto_After_Units_Is_A_Unique_Term()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse(".class { margin:0 auto .4em; }");

            var terms = style.RuleSets[0].Declarations[0].Expression.Terms;

            Assert.AreEqual(0, parser.Errors.Count);
            Assert.AreEqual("0", terms[0].Value);
            Assert.AreEqual("auto", terms[1].Value);
            Assert.AreEqual(".4", terms[2].Value);
            Assert.AreEqual("Em", terms[2].UnitString);
        }

        [Test]
        public void Auto_In_Last_Position_Is_A_Unique_Term()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse(".class { margin:0 auto .4em auto; }");

            var terms = style.RuleSets[0].Declarations[0].Expression.Terms;

            Assert.AreEqual(0, parser.Errors.Count);
            Assert.AreEqual("0", terms[0].Value);
            Assert.AreEqual("auto", terms[1].Value);
            Assert.AreEqual(".4", terms[2].Value);
            Assert.AreEqual("Em", terms[2].UnitString);
            Assert.AreEqual("auto", terms[3].Value);
        }

        [Test]
        public void Auto_Duplicated_Are_Unique()
        {
            var parser = new StylesheetParser();
            var style = parser.Parse(".class { margin:0 auto auto; }");

            var terms = style.RuleSets[0].Declarations[0].Expression.Terms;

            Assert.AreEqual(0, parser.Errors.Count);
            Assert.AreEqual("0", terms[0].Value);
            Assert.AreEqual("auto", terms[1].Value);
            Assert.AreEqual("auto", terms[2].Value);
        }
    }
}
