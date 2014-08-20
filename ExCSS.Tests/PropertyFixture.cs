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

        [Test]
        public void Force_Long_Html_Color()
        {
            var color = HtmlColor.FromHex("FF00FF");
            var colorString = color.ToString(true, false);

            Assert.AreEqual(colorString.Length, 7);
        }
    }
}
