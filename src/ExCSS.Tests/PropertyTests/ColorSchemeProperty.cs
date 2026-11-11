using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ColorSchemePropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("normal")]
        [InlineData("light")]
        [InlineData("dark")]
        [InlineData("light dark")]
        [InlineData("DARK LIGHT")]
        [InlineData("only dark")]
        [InlineData("dark only")]
        [InlineData("light dark only")]
        [InlineData("light my-scheme")]
        [InlineData("my-scheme")]
        public void ColorSchemeLegalValues(string value)
        {
            var property = ParseDeclaration($"color-scheme: {value}");

            Assert.Equal("color-scheme", property.Name);
            Assert.IsType<ColorSchemeProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal(value, ((ColorSchemeProperty)property).Value);
        }

        [Theory]
        [InlineData("color-scheme: only")]
        [InlineData("color-scheme: normal light")]
        [InlineData("color-scheme: light normal")]
        [InlineData("color-scheme: only light only")]
        [InlineData("color-scheme: light only dark")]
        [InlineData("color-scheme: only only light")]
        [InlineData("color-scheme: default light")]
        [InlineData("color-scheme: revert-layer light")]
        [InlineData("color-scheme: inherit light")]
        [InlineData("color-scheme: light, dark")]
        [InlineData("color-scheme: 1px")]
        [InlineData("color-scheme: \"dark\"")]
        public void ColorSchemeIllegalValues(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.IsType<ColorSchemeProperty>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("color-scheme: inherit")]
        [InlineData("color-scheme: initial")]
        [InlineData("color-scheme: unset")]
        public void ColorSchemeGlobalKeywords(string snippet)
        {
            Assert.True(ParseDeclaration(snippet).HasValue);
        }

        [Fact]
        public void ColorSchemeIsInherited()
        {
            Assert.True(ParseDeclaration("color-scheme: dark").CanBeInherited);
        }

        [Fact]
        public void ColorSchemeStyleDeclarationAccessor()
        {
            var style = ParseDeclarations("color-scheme: light dark");

            Assert.Equal("light dark", style.ColorScheme);
            style.ColorScheme = "dark";
            Assert.Equal("dark", style.ColorScheme);
        }
    }
}
