using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class PropertyFixture
    {
        [Test]
        public void Parser_Finds_Multiple_Properties()
        {
            var parser = new Parser();
            var css = parser.Parse(".class{border: solid 1px red;height: 5px} .other{margin: 0;}");

            var rules = css.Rulesets;

            Assert.AreEqual(2, rules.Count);
            Assert.AreEqual(2, rules[0].Declarations.Count);
        }
    }
}
