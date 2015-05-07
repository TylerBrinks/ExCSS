using System.Linq;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class StylesheetFixture
    {
        [Test]
        public void Empty_stylesheet_does_not_throw_on_ToString()
        {
            var parser = new Parser();
            var styleSheet = parser.Parse(string.Empty);
            styleSheet.ToString();
        }
    }
}
