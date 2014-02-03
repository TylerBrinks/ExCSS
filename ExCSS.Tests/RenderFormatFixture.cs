
using System;
using ExCSS.Tests.Properties;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class RenderFormatFixture
    {
        [Test]
        public void Stylesheet_Renders_Inline()
        {
            var parser = new Parser();
       
            var css = parser.Parse(Resources.Css3);

            Assert.AreEqual(Resources.Css3Min, css.ToString());
        }

        [Test]
        public void Stylesheet_Renders_Friendly_Format()
        {
            var parser = new Parser();
            var css = parser.Parse(Resources.Css3);

            Assert.AreEqual(Resources.Css3Friendly, css.ToString(true));
        }
    }
}
