using Xunit;

namespace ExCSS.Tests
{
    using ExCSS;
    using System.Linq;

    public class DebugTests
    {
        [Fact]
        public void Debug_stuff_here()
        {
            var sheet = new StylesheetParser().Parse("h1{ color: #abc; }");
            var x = sheet.ToCss();
            var a = x;

            var parser = new StylesheetParser();
            var stylesheet = parser.Parse(".someClass{color: red; background-image: url('/images/logo.png')");

            var rule = stylesheet.Rules.First() as StyleRule;
            var selector = rule.SelectorText; // Yields .someClass
            var color = rule.Style.Color; // red
            var image = rule.Style.BackgroundImage; // url('/images/logo.png')
        }
    }
}