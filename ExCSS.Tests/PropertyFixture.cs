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

            Assert.AreEqual(2, css.Rules.Count);
            Assert.AreEqual(2, css.StyleRules[0].Declarations.Count);
        }
    }
}
