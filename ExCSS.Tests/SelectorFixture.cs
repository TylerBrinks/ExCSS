using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class SelectorFixture
    {
        [Test]
        public void Parser_Reads_Global_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("*{}");

            var rules = css.Ruleset;

            Assert.AreEqual("*{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Element_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Empty_Attribute_Element_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Quoted_Attribute_Element_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo=\"bar\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Space_Separated_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo~=\"bar\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo~=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Starts_With_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo^=\"bar\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo^=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Ends_With_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo$=\"bar\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo$=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Contains_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo*=\"bar\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo*=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Dash_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo|=\"bar\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo|=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Multiple_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo=\"bar\"][rel=\"important\"]{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E[foo=\"bar\"][rel=\"important\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E:pseudo{}");

            var rules = css.Ruleset;

            Assert.AreEqual("E:pseudo{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Functions()
        {
            var parser = new Parser();
            var css = parser.Parse("E:nth-child(n){}");

            var rules = css.Ruleset;

            Assert.AreEqual("E:nth-child(n){}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Functions_With_Rules()
        {
            var parser = new Parser();
            var css = parser.Parse("E:nth-child(3n+2){}");

            var rules = css.Ruleset;

            Assert.AreEqual("E:nth-child(3n+2){}", rules[0].ToString());
        }
    }
}
