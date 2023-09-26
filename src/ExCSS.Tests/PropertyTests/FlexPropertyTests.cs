using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class FlexPropertyTests
    {

        [Fact]
        public void JustifyAlign_Parses()
        {
            string css = """
html {
    justify-content: center;
    align-items: center;
    align-content: center;
    align-self: center;
}
""";
            var stylesheet = new StylesheetParser().Parse(css);

            var info = stylesheet.StyleRules.First() as ExCSS.StyleRule;

            Assert.Equal(@"center", info.Style.AlignItems);
            Assert.Equal(@"center", info.Style.AlignContent);
            Assert.Equal(@"center", info.Style.AlignSelf);
            Assert.Equal(@"center", info.Style.JustifyContent);
        }
    }
}
