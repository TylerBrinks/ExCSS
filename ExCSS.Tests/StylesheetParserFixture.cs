using System.Reflection;
using NUnit.Framework;
using ExCSS.Model;

namespace ExCSS.Tests
{
    [TestFixture]
    public class ExCSSFixture
    {
        private readonly dynamic _stylesheets = new Stylesheets();
        private readonly StylesheetParser _parser;
        private readonly Stylesheet _parsed;

        public ExCSSFixture()
        {
            _parser = new StylesheetParser();
            _parsed = _parser.Parse(_stylesheets.Css3);
        }

        [Test]
        public void Parser_Loads_Styles_From_Strings()
        {
            Assert.DoesNotThrow(() => new StylesheetParser().Parse(_stylesheets.Css3));
        }

        [Test]
        public void Parser_Loads_Styles_From_Steams()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ExCSS.Tests.Stylesheets.Css3.css");

            Assert.DoesNotThrow(() => new StylesheetParser().Parse(stream));
        }

        [Test]
        public void Parser_Loads_All_Styles()
        {
            var rules = _parsed.RuleSets;

            Assert.AreEqual(70, rules.Count);
            Assert.AreEqual(0, _parser.Errors.Count);
        }

        [Test]
        public void Parser_Loads_IE_Hacks()
        {
            const string ieHack = @".canvas {
                                       *display: inline;
                                    }";

            var parser = new StylesheetParser();
            var stylesheet = parser.Parse(ieHack);

            var rules = stylesheet.RuleSets;

            Assert.AreEqual(1, rules[0].Declarations.Count);
            Assert.IsTrue(rules[0].Declarations[0].Name.StartsWith("*"));
            Assert.AreEqual(0, parser.Errors.Count);
        }

        [Test]
        public void G()
        {
            var parser = new StylesheetParser();
            var stylesheet = parser.Parse(
                "@import url(other.css) screen; @import url(chained.css) screen; .rule{ background:url('../../virtualpath.gif');}"
                );
            stylesheet.ToString();
        }
    }
}
