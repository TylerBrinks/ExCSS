namespace ExCSS.Tests
{
    using Xunit;

    public class ShadowGrammarTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("1px 2px")]
        [InlineData("1px 2px 3px")]
        [InlineData("1px 2px 3px 4px")]
        [InlineData("1px 2px 0 -4px")]
        [InlineData("red 1px 2px")]
        [InlineData("1px 2px red")]
        [InlineData("inset 1px 2px red")]
        [InlineData("1px 2px red inset")]
        [InlineData("red inset 1px 2px 3px 4px")]
        [InlineData("inset red 1px 2px")]
        [InlineData("1px 2px 3px red, inset 0 0 5px 1px blue")]
        [InlineData("none")]
        public void BoxShadowLegal(string value)
        {
            var property = ParseDeclaration("box-shadow: " + value);
            Assert.IsType<BoxShadowProperty>(property);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("1px")]
        [InlineData("1px 2px -3px red")]
        [InlineData("1px 2px -3px")]
        [InlineData("1px 2px 3px 4px 5px")]
        [InlineData("1px red 2px")]
        [InlineData("1px 2px red 3px")]
        [InlineData("1px 2px inset 3px")]
        [InlineData("inset inset 1px 2px")]
        [InlineData("red blue 1px 2px")]
        [InlineData("red")]
        [InlineData("inset")]
        [InlineData("none, 1px 2px")]
        [InlineData("1px 2px,")]
        [InlineData("1px 2px, , 3px 4px")]
        public void BoxShadowIllegal(string value)
        {
            var property = ParseDeclaration("box-shadow: " + value);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("1px 2px")]
        [InlineData("1px 2px 3px")]
        [InlineData("1px 2px 0")]
        [InlineData("red 1px 2px")]
        [InlineData("1px 2px 3px red")]
        [InlineData("1px 2px red, 3px 4px blue")]
        [InlineData("none")]
        public void TextShadowLegal(string value)
        {
            var property = ParseDeclaration("text-shadow: " + value);
            Assert.IsType<TextShadowProperty>(property);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("1px")]
        [InlineData("1px 2px 3px 4px red")]
        [InlineData("1px 2px 3px 4px")]
        [InlineData("1px 2px -3px red")]
        [InlineData("inset 1px 2px red")]
        [InlineData("1px 2px red inset")]
        [InlineData("1px red 2px")]
        [InlineData("1px 2px red 3px")]
        [InlineData("red blue 1px 2px")]
        [InlineData("none, 1px 2px")]
        public void TextShadowIllegal(string value)
        {
            var property = ParseDeclaration("text-shadow: " + value);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("box-shadow", "1px 2px 3px 4px rgb(255, 0, 0)", "1px 2px 3px 4px rgb(255, 0, 0)")]
        [InlineData("box-shadow", "inset 1px 2px", "inset 1px 2px")]
        [InlineData("text-shadow", "1px 2px 3px rgb(255, 0, 0), 0 0 2px rgb(0, 0, 255)", "1px 2px 3px rgb(255, 0, 0), 0 0 2px rgb(0, 0, 255)")]
        public void ShadowRoundTrip(string name, string value, string expected)
        {
            var property = ParseDeclaration(name + ": " + value);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }
    }
}
