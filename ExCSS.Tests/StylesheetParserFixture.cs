using ExCSS.Tests.Properties;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class StylesheetParserFixture
    {
        [Test]
        public void Parser_Reads_Imports()
        {
            var parser = new Parser();
            var css = parser.Parse(Resources.Css3);

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url('style.css');", imports[0].ToString());
            Assert.AreEqual("@import url('style.css');", imports[1].ToString());
            Assert.AreEqual("@import url('style.css') print;", imports[2].ToString());
            Assert.AreEqual("@import url('style.css') projection, tv;", imports[3].ToString());
            Assert.AreEqual("@import url('style.css') handheld and (max-width: 400px);", imports[4].ToString());
            Assert.AreEqual("@import url('style.css') screen 'Plain style';", imports[5].ToString());
            Assert.AreEqual("@import url('style.css') 'Four-columns and dark';", imports[6].ToString());
            Assert.AreEqual("@import url('style.css') 'Style Sheet';", imports[7].ToString());
        }
    }
}
