using System.IO;
using ExCSS.Model;
using ExCSS.Tests.Properties;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class ExCSSFixture
    {
        private readonly StylesheetParser _parser;
        private Stylesheet _parsed;

        public ExCSSFixture()
        {
            _parser = new StylesheetParser();
            

            //new StylesheetParser().Parse(".big{color: red;}");
            //new StylesheetParser().Parse("@import \"mystyle.css\";");
        }

        [Test]
        public void Parser_Loads_Styles_From_Strings()
        {
            var parsed = _parser.Parse(Resources.Selectors);
            var r = parsed;
            //Assert.DoesNotThrow(() => new StylesheetParser().Parse(_stylesheets.Css3));
        }

        [Test]
        public void Parser_Work()
        {
            //new StylesheetParser().Parse("E>F{color: blue;}");
            //_parser.Parse(_stylesheets.Css3);
        }

        //[Test]
        //public void Parser_Loads_Styles_From_Steams()
        //{
        //    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ExCSS.Tests.Stylesheets.Css3.css");

        //    Assert.DoesNotThrow(() => new StylesheetParser().Parse(stream));
        //}

        [Test]
        public void Parser_Loads_All_Styles()
        {
            var rules = _parsed.RuleSets;

            Assert.AreEqual(70, rules.Count);
            //Assert.AreEqual(0, _parser.Errors.Count);
        }

        [Test]
        public void Parser_Loads_JqueryUI_Styles()
        {
            // It's a good test of the tool since it has a diversity of selector 
            // and term arrangements. Plus it's widely used

            //_parsed = _parser.Parse(_stylesheets.JquerUI);

            //Assert.AreEqual(0, _parser.Errors.Count);
        }
    }
}
