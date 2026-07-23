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

        [Fact]
        public void FlexAuto_Parses()
        {
            string css = """
html {
    flex: auto;
}
""";

            var stylesheet = new StylesheetParser().Parse(css);
            var info = stylesheet.StyleRules.First() as ExCSS.StyleRule;
            Assert.Equal(@"1 1 auto", info.Style.Flex);
        }

        [Theory]
        // flex-flow is "<'flex-direction'> || <'flex-wrap'>", so either order is valid and both normalize
        // to the canonical direction-then-wrap value.
        [InlineData("row wrap", "row wrap", "row", "wrap")]
        [InlineData("wrap row", "row wrap", "row", "wrap")]
        [InlineData("column nowrap", "column nowrap", "column", "nowrap")]
        [InlineData("nowrap column", "column nowrap", "column", "nowrap")]
        [InlineData("wrap-reverse row-reverse", "row-reverse wrap-reverse", "row-reverse", "wrap-reverse")]
        [InlineData("column-reverse wrap", "column-reverse wrap", "column-reverse", "wrap")]
        public void FlexFlowAcceptsBothValuesInEitherOrder(string value, string expected,
            string direction, string wrap)
        {
            var property = ParseDeclaration("flex-flow: " + value);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);

            var style = ParseDeclarations("flex-flow: " + value);
            Assert.Equal(direction, style.FlexDirection);
            Assert.Equal(wrap, style.FlexWrap);
        }

        [Theory]
        // A single value is still valid on its own.
        [InlineData("row")]
        [InlineData("wrap")]
        [InlineData("column-reverse")]
        public void FlexFlowAcceptsSingleValue(string value)
        {
            var property = ParseDeclaration("flex-flow: " + value);

            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        // "||" still means at most one occurrence of each operand.
        [InlineData("row row")]
        [InlineData("wrap wrap")]
        [InlineData("row column")]
        [InlineData("bogus wrap")]
        [InlineData("wrap bogus")]
        public void FlexFlowRejectsInvalidCombinations(string value)
        {
            var property = ParseDeclaration("flex-flow: " + value);

            Assert.False(property.HasValue);
        }

        private static StyleDeclaration ParseDeclarations(string declarations)
        {
            var parser = new StylesheetParser();
            var style = new StyleDeclaration(parser);
            style.Update(declarations);
            return style;
        }

        private static Property ParseDeclaration(string source)
        {
            return new StylesheetParser().ParseDeclaration(source);
        }
    }
}
