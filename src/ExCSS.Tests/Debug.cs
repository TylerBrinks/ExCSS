using System;
using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    using ExCSS;

    public class DebugTests
    {
        [Fact]
        public void Debug_stuff_here()
        {
            var sheet = new StylesheetParser().Parse("foo > bar {color: red; }");
            var _ = sheet.ToCss();
            Console.WriteLine(sheet.StyleRules.First().SelectorText);
        }
    }
}